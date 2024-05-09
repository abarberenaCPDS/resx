using Host;
using System.ServiceModel;
using System.Windows.Forms;

[ServiceContract]
interface IMyContract
{
    [OperationContract]
    void MyMethod();
}

class MyService : IMyContract
{
    private int _counter = 0;

    public void MyMethod()
    {
        Form1 form = Application.OpenForms[0] as Form1;
        form.Counter = ++_counter;
    }
}