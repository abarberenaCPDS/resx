using ServiceModelEx.ServiceBus;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows.Forms;
using Microsoft.ServiceBus;

namespace Host
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ServiceHost host = new ServiceBusHost(typeof(MyService));
            string k = "RootManageSharedAccessKey";
            string v = "Qp1wMzt1tF6x1z0yn9FBF0zLbw/6UagB6974nqHaiVU=";
            host.SetServiceBusCredentials(k, v);
            host.Open();
            Application.Run(new Form1());

            host.Close();
        }
    }
}