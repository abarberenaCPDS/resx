using System.Diagnostics;
using Veros.Crypto.Ecdsa.SignButVerify;

namespace TestClient
{
    [TestClass]
    public class PerfomanceUnitTest
    {
        [TestMethod]
        public void Test_MeasureTxPerSec()
        {
            string privateKeyFile;
            string privateKeyFilePwd;
            string dataFileStr;
            byte[] dataFileRaw;
            byte[] signatureRaw;
            string signatureStr;
            byte[] signingKeyRaw;

            //string publicKeyFile;
            //byte[] publicKeyFileRaw;
            //bool isValid;

            //ISignButVerify iSign;
            ISigningKeyFactory signingKeyFactory;


            // arrange
            privateKeyFile = @"prv.pem";
            privateKeyFilePwd = string.Empty;
            string certificateSerialNumber = "3bb1f896d61260f39d1a5a8777349105b920beb4";
            dataFileStr = @"sample.json";
            signatureStr = string.Empty;

            ////using a PEM key
            //publicKeyFile = "vselect-devqapub.pem";

            // act
            dataFileRaw = File.ReadAllBytes(dataFileStr);

            int iterations = 100;
            int threads = Environment.ProcessorCount;

            for (int i = 0; i < threads; i++)
            {
                Thread thread = new Thread(delegate ()
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    // using the signing key factory
                    signingKeyFactory = SigningKeyFactory.CreateFor<SigningKey, ISigningKeyFactory>(certificateSerialNumber, privateKeyFilePwd);

                    for (int j = 0; j < iterations; j++)
                    {
                        signatureRaw = signingKeyFactory.Sign(dataFileRaw);
                        signatureStr = Convert.ToBase64String(signatureRaw);
                    }
                    sw.Stop();

                    int txPerSec = (int)(Convert.ToDecimal(iterations) / Convert.ToDecimal(sw.ElapsedMilliseconds) * 1000);
                    double txDuration = 1.0 / (double)txPerSec;
                    Trace.WriteLine($"Tx/Sec: {txPerSec * threads} <---> Tx Duration: {txDuration}.", "Stress test results");
                });
                thread.Start();
            }

            // assert
            Assert.IsNotNull(string.IsNullOrEmpty(signatureStr));
        }
    }
}