using ServiceModelEx.ServiceBus;
using System;
using System.Configuration;
using System.ServiceModel;
using System.Windows.Forms;

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

            string sbKeyName = ConfigurationManager.AppSettings["sb-key-name"];
            string sbKeyValue = ConfigurationManager.AppSettings["sb-key-value"];

            host.SetServiceBusCredentials(sbKeyName, sbKeyValue);
            host.Open();

            Application.Run(new frmHost());
            host.Close();
        }
    }
}