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
        string v = "KEY";
        m_Proxy.SetServiceBusCredentials(k, v);
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
