// © 2011 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.ServiceBus;
using System.Windows.Forms;
using ServiceModelEx.ServiceBus;

class Program
{
    static void Main()
    {
        ServiceBusHost host = new ServiceBusHost(typeof(MyService));
        string k = "RootManageSharedAccessKey";
        string v = "BDfLYPw6/dH8x56jnEXmmafsD/VeAh8lCwMREzd5QGE=";
        host.SetServiceBusCredentials(k, v);
        host.ConfigureAnonymousMessageSecurity("localhost");

        HostForm form = new HostForm();

        host.Open();

        Application.Run(form);

        host.Close();
    }
}
