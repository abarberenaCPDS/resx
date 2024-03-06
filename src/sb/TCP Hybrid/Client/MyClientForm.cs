using System;
using System.Windows.Forms;
using Microsoft.ServiceBus;
using System.ServiceModel;
using ServiceModelEx.ServiceBus;

partial class MyClientForm : Form
{
    MyContractClient m_Proxy;

    public MyClientForm(string secret)
    {
        InitializeComponent();

        m_Proxy = new MyContractClient();
        string k = "RootManageSharedAccessKey";
        //string v = "Qp1wMzt1tF6x1z0yn9FBF0zLbw/6UagB6974nqHaiVU=";
        string v = "BDfLYPw6/dH8x56jnEXmmafsD/VeAh8lCwMREzd5QGE=";
        m_Proxy.SetServiceBusCredentials(k, v);
        //m_Proxy.SetServiceBusCredentials(secret);
    }

    void OnCallService(object sender, EventArgs e)
    {
        m_Proxy.MyMethod();
    }

    void OnClosed(object sender, FormClosedEventArgs e)
    {
        m_Proxy.Close();
    }
}
