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
        string v = "";
        host.SetServiceBusCredentials(k, v);
        host.ConfigureAnonymousMessageSecurity("localhost");

        HostForm form = new HostForm();

        host.Open();

        Application.Run(form);

        host.Close();
    }
}
