using System;
using System.ServiceModel;
using ServiceModelEx.ServiceBus;

[ServiceContract]
interface IMyContract
{
    [OperationContract]
    void MyMethod();
}

class MyContractClient : ServiceBusClientBase<IMyContract>, IMyContract
{
    public void MyMethod()
    {
        Channel.MyMethod();
    }
}
