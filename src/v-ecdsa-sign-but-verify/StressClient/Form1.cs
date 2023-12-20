using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Veros.Crypto.Ecdsa.SignButVerify;

namespace StressClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.txtThreads.Text = Environment.ProcessorCount.ToString();
        }

        private void btnRunTest_Click(object sender, EventArgs e)
        {
            int iterations = Convert.ToInt32(this.txtIterations.Text);
            int threads = Convert.ToInt32(this.txtThreads.Text);

            ISigningKeyFactory signingKeyFactory;
            string certificateSerialNumber = "3bb1f896d61260f39d1a5a8777349105b920beb4";

            string dataFileStr;
            byte[] dataFileRaw;
            byte[] signatureRaw;
            string signatureStr;
            string privateKeyFilePwd = string.Empty;



            for (int i = 0; i < threads; i++)
            {
                Thread thread = new Thread(delegate ()
                {

                    signingKeyFactory = SigningKeyFactory.CreateFor<SigningKey, ISigningKeyFactory>(certificateSerialNumber, privateKeyFilePwd);

                    Stopwatch sw = Stopwatch.StartNew();
                    for (int j = 0; j < iterations; j++)
                    {
                        //Trace.WriteLine($"iteration: {j + 1}");
                        dataFileStr = j % 2 == 1 ? @"sample.json" : @"sample-tampered.json";
                        dataFileRaw = File.ReadAllBytes(dataFileStr);

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
        }
    }
}