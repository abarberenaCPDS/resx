 
 


using System;
using System.Diagnostics;
using System.Reflection;
using System.ServiceModel;
using System.Threading;

namespace ServiceModelEx
{
   public abstract class PublishService<T> where T : class
   {
      protected static void FireEvent(params object[] args)
      {
         string action = OperationContext.Current.IncomingMessageHeaders.Action;
         string[] slashes = action.Split('/');
         string methodName = slashes[slashes.Length-1];

         FireEvent(methodName,args);
      }
      static void FireEvent(string methodName,object[] args)
      {
         PublishPersistent(methodName,args);
         PublishTransient(methodName,args);
      }
      static void PublishPersistent(string methodName,object[] args)
      {
         T[] subscribers = SubscriptionManager<T>.GetPersistentList(methodName);
         Publish(subscribers,true,methodName,args);
      }
      static void PublishTransient(string methodName,object[] args)
      {
         T[] subscribers = SubscriptionManager<T>.GetTransientList(methodName);
         Publish(subscribers,false,methodName,args);
      }
      static void Publish(T[] subscribers,bool closeSubscribers,string methodName,object[] args)
      {
         WaitCallback fire = (subscriber)=>
                             {
                                Invoke(subscriber as T,methodName,args);
                                if(closeSubscribers)
                                {
                                   try
                                   {
                                      using(subscriber as IDisposable)
                                      {
                                      }
                                   }
                                   catch
                                   {}
                                }
                             };
         Action<T> queueUp = (subscriber)=>
                             {
                                ThreadPool.QueueUserWorkItem(fire,subscriber);
                             };
         subscribers.ForEach(queueUp);
      }
      static void Invoke(T subscriber,string methodName,object[] args)
      {
         Debug.Assert(subscriber != null);
         Type type = typeof(T);
         MethodInfo methodInfo = type.GetMethod(methodName);
         try
         {
            methodInfo.Invoke(subscriber,args);
         }
         catch(Exception e)
         {
            Trace.WriteLine(e.Message);
         }
      }
   }
}