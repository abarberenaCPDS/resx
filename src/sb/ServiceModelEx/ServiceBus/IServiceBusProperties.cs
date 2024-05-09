 
 


using System;
using Microsoft.ServiceBus;

namespace ServiceModelEx.ServiceBus
{
   public interface IServiceBusProperties
   {
      TransportClientEndpointBehavior Credential
      {get;set;}

      Uri[] Addresses
      {get;}
   }
}