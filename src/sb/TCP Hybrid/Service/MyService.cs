using System;
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
   public void MyMethod()
   {
      MessageBox.Show("MyMethod()","MyService");
   }
}
