 
 



using System;
using System.ServiceModel.Persistence;

namespace ServiceModelEx
{
   public class TransactionalInstanceProviderFactory : MemoryProviderFactory
   {
      public override PersistenceProvider CreateProvider(Guid id)
      {
         return new TransactionalInstanceProvider(id);
      }
   }
}