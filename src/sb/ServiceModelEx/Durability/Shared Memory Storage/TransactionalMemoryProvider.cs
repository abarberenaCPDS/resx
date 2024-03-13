 
 


using System;

namespace ServiceModelEx
{
   public class TransactionalMemoryProvider : MemoryProvider
   {
      public TransactionalMemoryProvider(Guid id) : base(id,new TransactionalMemoryStore<Guid,object>())
      {}
   }
}
