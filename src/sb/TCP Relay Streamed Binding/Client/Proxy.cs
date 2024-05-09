using System.IO;
using System.ServiceModel;

[ServiceContract]
interface IMyContract
{
    [OperationContract]
    Stream StreamMusic(string clip);
}

class MyContractClient : ClientBase<IMyContract>, IMyContract
{
    public Stream StreamMusic(string clip)
    {
        return base.Channel.StreamMusic(clip);
    }
}