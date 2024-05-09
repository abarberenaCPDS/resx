using System.ServiceModel;

[ServiceContract]
interface IMyContract
{
    [OperationContract]
    void MyMethod();
}

class MyContractClient : ClientBase<IMyContract>, IMyContract
{
    public void MyMethod()
    {
        base.Channel.MyMethod();
    }
}