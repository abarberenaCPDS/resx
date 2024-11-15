 
 


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Dispatcher;

namespace ServiceModelEx
{
   public static class InProcFactory
   {
      static readonly string BaseAddress = "net.pipe://localhost/" + Guid.NewGuid();

      static readonly Binding Binding;

      static Dictionary<Type,Dictionary<Type,Tuple<ServiceHost,EndpointAddress>>> m_Hosts = new Dictionary<Type,Dictionary<Type,Tuple<ServiceHost,EndpointAddress>>>();
      static Dictionary<Type,ServiceThrottlingBehavior> m_Throttles = new Dictionary<Type,ServiceThrottlingBehavior>();
      static Dictionary<Type,object> m_Singletons = new Dictionary<Type,object>();

      static InProcFactory()
      {
         NetNamedPipeBinding binding;
         try
         {
            binding = new NetNamedPipeContextBinding("InProcFactory");
         }
         catch
         {
            binding = new NetNamedPipeContextBinding();
         }

         binding.TransactionFlow = true;
         Binding = binding;
         binding.MaxReceivedMessageSize *= 4;

         AppDomain.CurrentDomain.ProcessExit += delegate
                                                {
                                                   foreach(Dictionary<Type,Tuple<ServiceHost,EndpointAddress>> endpoints in m_Hosts.Values)
                                                   {
                                                      foreach(Tuple<ServiceHost,EndpointAddress> record in endpoints.Values)
                                                      {
                                                         record.Item1.Close();
                                                      }
                                                   }
                                                };
      }

      /// <summary>
      /// Can only call SetThrottle() before creating any instance of the service
      /// </summary>
      /// <typeparam name="S">Service type</typeparam>
      /// <param name="throttle">Throttle to use</param>
      [MethodImpl(MethodImplOptions.Synchronized)]
      public static void SetThrottle<S>(ServiceThrottlingBehavior throttle)
      {
         m_Throttles[typeof(S)] = throttle;
      }
      /// <summary>
      /// Can only call MaxThrottle() before creating any instance of the service
      /// </summary>
      public static void MaxThrottle<S>()
      {
         SetThrottle<S>(Int32.MaxValue,Int32.MaxValue,Int32.MaxValue);
      }
      /// <summary>
      /// Can only call SetThrottle() before creating any instance of the service
      /// </summary>
      public static void SetThrottle<S>(int maxCalls,int maxSessions,int maxInstances)
      {
         ServiceThrottlingBehavior throttle = new ServiceThrottlingBehavior();
         throttle.MaxConcurrentCalls = maxCalls;
         throttle.MaxConcurrentSessions = maxSessions;
         throttle.MaxConcurrentInstances = maxInstances;
         SetThrottle<S>(throttle);
      }
      /// <summary>
      /// Can only call SetSingleton() before creating any instance of the service
      /// </summary>
      /// <typeparam name="S"></typeparam>
      /// <param name="singleton"></param>
      [MethodImpl(MethodImplOptions.Synchronized)]
      public static void SetSingleton<S>(S singleton)
      {
         m_Singletons.Add(typeof(S),singleton);
      }

      [MethodImpl(MethodImplOptions.Synchronized)]
      public static I CreateInstance<S,I>()
         where I : class
         where S : class,I
      {
         EndpointAddress address = GetAddress<S,I>();
         ChannelFactory<I> factory = new ChannelFactory<I>(Binding,address);
         factory.AddGenericResolver();

         return factory.CreateChannel();
      }
      [MethodImpl(MethodImplOptions.Synchronized)]
      public static I CreateInstance<S,I,C>(InstanceContext<C> context)
         where I : class
         where S : class,I
      {
         EndpointAddress address = GetAddress<S,I>();
         DuplexChannelFactory<I,C> factory = new DuplexChannelFactory<I,C>(context,Binding,address);
         factory.AddGenericResolver();
         return factory.CreateChannel();
      }
      [MethodImpl(MethodImplOptions.Synchronized)]
      public static I CreateInstance<S,I,C>(C callback)
         where I : class
         where S : class,I
      {
         DuplexClientBase<I,C>.VerifyCallback();
         InstanceContext<C> context = new InstanceContext<C>(callback);
         return CreateInstance<S,I,C>(context);
      }

      public static I CreateInstance<S,I>(IEndpointBehavior clientBehavior)
         where I : class
         where S : class,I
      {
         EndpointAddress address = GetAddress<S,I>();
         ChannelFactory<I> factory = new ChannelFactory<I>(Binding,address);
         factory.Endpoint.EndpointBehaviors.Add(clientBehavior);
         factory.AddGenericResolver();

         return factory.CreateChannel();
      }
      public static I CreateInstance<S,I>(Assembly callingAssembly,IErrorHandler errorHandler)
         where I : class
         where S : class,I
      {
         EndpointAddress address = GetAddress<S,I>(callingAssembly,errorHandler);
         ChannelFactory<I> factory = new ChannelFactory<I>(Binding,address);
         factory.AddGenericResolver(callingAssembly);

         return factory.CreateChannel();
      }
      public static I CreateInstance<S,I>(IEndpointBehavior clientBehavior,Assembly callingAssembly,IErrorHandler errorHandler)
         where I : class
         where S : class,I
      {
         EndpointAddress address = GetAddress<S,I>(callingAssembly,errorHandler);
         ChannelFactory<I> factory = new ChannelFactory<I>(Binding,address);
         factory.Endpoint.EndpointBehaviors.Add(clientBehavior);
         factory.AddGenericResolver(callingAssembly);

         return factory.CreateChannel();
      }

      static Type[] GetContracts<S>()
      {
         Debug.Assert(typeof(S).IsClass);

         Type[] interfaces = typeof(S).GetInterfaces();

         List<Type> contracts = new List<Type>();

         foreach(Type type in interfaces)
         {
            if(type.GetCustomAttributes(typeof(ServiceContractAttribute),false).Any())
            {
               contracts.Add(type);
            }
         }

         return contracts.ToArray();
      }
      static EndpointAddress GetAddress<S,I>()
         where I : class
         where S : class,I
      {
         return GetAddress<S,I>(null,null);
      }
      static EndpointAddress GetAddress<S,I>(Assembly callingAssembly,IErrorHandler errorHandler)
         where I : class
         where S : class,I
      {
         if(m_Hosts.ContainsKey(typeof(S)))
         {
            Debug.Assert(m_Hosts[typeof(S)].ContainsKey(typeof(I)));

            return m_Hosts[typeof(S)][typeof(I)].Item2;
         }
         else
         {
            ServiceHost<S> host;
            if(m_Singletons.ContainsKey(typeof(S)))
            {
               S singleton = m_Singletons[typeof(S)] as S;
               Debug.Assert(singleton != null);
               host = new ServiceHost<S>(singleton);
            }
            else
            {
               host = new ServiceHost<S>();
            }

            Type[] contracts = GetContracts<S>();
            Debug.Assert(contracts.Any());

            m_Hosts[typeof(S)] = new Dictionary<Type,Tuple<ServiceHost,EndpointAddress>>();

            foreach(Type contract in contracts)
            {
               string address = BaseAddress + Guid.NewGuid() + "_" + contract;

               m_Hosts[typeof(S)][contract] = new Tuple<ServiceHost,EndpointAddress>(host,new EndpointAddress(address));
               host.AddServiceEndpoint(contract,Binding,address);
            }

            if(m_Throttles.ContainsKey(typeof(S)))
            {
               host.SetThrottle(m_Throttles[typeof(S)]);
            }
            host.AddGenericResolver(callingAssembly);
            if(errorHandler != null)
            {
               host.AddErrorHandler(errorHandler);
            }
            host.Open();
         }
         return GetAddress<S,I>(callingAssembly,errorHandler);
      }
      public static void CloseProxy<I>(I instance) where I : class
      {
         ICommunicationObject proxy = instance as ICommunicationObject;
         Debug.Assert(proxy != null);
         proxy.Close();
      }
   }
}