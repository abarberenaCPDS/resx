using System;
using System.Windows.Forms;
using ServiceModelEx.ServiceBus;

namespace Client
{
    public partial class Form1 : Form
    {
        private MyContractClient proxy;

        public Form1()
        {
            InitializeComponent();
            string k = "RootManageSharedAccessKey";
            string v = "Qp1wMzt1tF6x1z0yn9FBF0zLbw/6UagB6974nqHaiVU=";

            this.proxy = new MyContractClient();
            this.proxy.SetServiceBusCredentials(k, v);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            proxy.MyMethod();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.proxy.Close();
        }
    }
}
