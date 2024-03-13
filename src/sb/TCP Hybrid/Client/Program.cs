using System;
using System.ServiceModel;
using Microsoft.ServiceBus;
using System.Windows.Forms;
using ServiceModelEx.ServiceBus;


class Program
{
   static void Main(string[] args)
   {
      const string secret = "KEY";
      MyClientForm client = new MyClientForm(secret);

      Application.Run(client);

   }
}
