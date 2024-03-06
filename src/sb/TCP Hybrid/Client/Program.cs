// © 2011 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using Microsoft.ServiceBus;
using System.Windows.Forms;
using ServiceModelEx.ServiceBus;


class Program
{
   static void Main(string[] args)
   {
      const string secret = "BDfLYPw6/dH8x56jnEXmmafsD/VeAh8lCwMREzd5QGE=";
      MyClientForm client = new MyClientForm(secret);

      Application.Run(client);

   }
}
