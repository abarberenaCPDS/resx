using System.IO;
using System.ServiceModel;

[ServiceContract]
interface IMyContract
{
    [OperationContract]
    Stream StreamMusic(string clip);
}

class MyService : IMyContract
{

    public Stream StreamMusic(string clip)
    {
        string assetsFolder = $@"{Directory.GetCurrentDirectory()}\Resources";
        string filepath = System.IO.Path.Combine(assetsFolder, clip);
        return new FileStream(filepath, FileMode.Open, FileAccess.Read);
    }
}