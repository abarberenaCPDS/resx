 
 


using System;

namespace ServiceModelEx
{
   [AttributeUsage(AttributeTargets.Class)]
   public class CallbackThreadAffinityBehaviorAttribute : CallbackThreadPoolBehaviorAttribute
   {
      public CallbackThreadAffinityBehaviorAttribute(Type clientType) : this(clientType,"Callback Worker Thread")
      {}
      public CallbackThreadAffinityBehaviorAttribute(Type clientType,string threadName) : base(1,clientType,threadName)
      {}
   }
}