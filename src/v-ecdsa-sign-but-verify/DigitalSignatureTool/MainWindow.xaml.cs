using NetEcdsaSignButVerify.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Veros.Crypto.Ecdsa.SignButVerify;

namespace DigitalSignatureTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public DigitalSignatureSettings _appSettings;

        public MainWindow()
        {
            InitializeComponent();
            PopulateSettings();
        }

        public void PopulateSettings()
        {
            // read settings
            string settingsFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Settings\DigitalSignatureToolSettings.json");
            _appSettings = JsonConvert.DeserializeObject<DigitalSignatureSettings>(File.ReadAllText(settingsFilePath));

            // populate controls
            //txtPrivateKey.Text = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _appSettings.PrivateKey).ToString();
            txtPrivateKey.Text = _appSettings.PrivateKey;
            txtPublicKey.Text = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _appSettings.PublicKey).ToString();
            txtJsonFile.Text = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ResponseFiles", _appSettings.JsonFile).ToString();
            txtJsonFileTampered.Text = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ResponseFiles", _appSettings.JsonFileTampered).ToString();

            lblVerifyResult.Content = "";
            lblVerifyAlternateResult.Content = "";
        }

        private void BtnSignFile_Click(object sender, RoutedEventArgs e)
        {
            Stress();
            //txtSignature.Text = this.GetSignature();
            //lblVerifyResult.Content = lblVerifyAlternateResult.Content = "";
        }

        private void Stress()
        {
            int iterations = 100;
            int threads = Environment.ProcessorCount;
            Stopwatch sw = Stopwatch.StartNew();


            byte[] contentRaw = File.ReadAllBytes(txtJsonFile.Text);
            string privateKeyStr = this.txtPrivateKey.Text;
            byte[] privateKeyRaw;
            byte[] signedRaw;
            string signature;


            sw.Start();

            for (int i = 0; i < threads; i++)
            {
                Thread thread = new Thread(delegate ()
                {
                    ISignButVerify signButVerify = new SignButVerify();
                    privateKeyRaw = signButVerify.LoadPrivateKeyFile(privateKeyStr, string.Empty);

                    Dispatcher.BeginInvoke(new Action(() =>
                    {


                        for (int j = 0; j < iterations; j++)
                        {
                            /// Sign data....
                            signedRaw = signButVerify.Sign(privateKeyRaw, contentRaw);
                            signature = Convert.ToBase64String(signedRaw);
                        }

                        sw.Stop();
                        int txPerSec = (int)((Convert.ToDecimal(iterations) / Convert.ToDecimal(sw.ElapsedMilliseconds)) * 1000);
                        double txDuration = 1.0 / (double)txPerSec;

                        Trace.WriteLine($"tx/sec {txPerSec * threads} <---> tx Duration {txDuration}");
                    }));

                });

                thread.Start();
            }


        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.txtSignature.Text);
        }

        private string GetSignature()
        {
            ISignButVerify signButVerify = new SignButVerify();
            byte[] contentRaw = File.ReadAllBytes(txtJsonFile.Text);

            //contentRaw = FileToByteArray(txtJsonFile.Text);

            string contents = File.ReadAllText(txtJsonFile.Text);
            byte[] contentRaw1 = Encoding.UTF8.GetBytes(contents);

            string privateKeyStr = this.txtPrivateKey.Text;
            byte[] privateKeyRaw = signButVerify.LoadPrivateKeyFile(privateKeyStr, string.Empty);
            byte[] signedRaw;
            string signature;

            /// Sign data....
            signedRaw = signButVerify.Sign(privateKeyRaw, contentRaw);

            // var digSig=ProxyForSigning<ISignButVerify>.Sign(content,"Customer specific params");

            signature = Convert.ToBase64String(signedRaw);
            return signature;
        }

        public byte[] FileToByteArray(string fileName)
        {
            byte[] fileContent = null;
            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fs);
            long byteLength = new System.IO.FileInfo(fileName).Length;
            fileContent = binaryReader.ReadBytes((Int32)byteLength);
            fs.Close();
            fs.Dispose();
            binaryReader.Close();
            return fileContent;
        }

        private bool VerifySignature(string signature, string fileLocation)
        {
            try
            {
                string publicKeyStr = this.txtPublicKey.Text;

                byte[] contentRaw = File.ReadAllBytes(fileLocation);
                byte[] signatureRaw = Convert.FromBase64String(signature);

                IVerify signButVerify = new SignButVerify();
                byte[] publicKeyRaw = signButVerify.LoadPublicKey(publicKeyStr);

                bool isValid = signButVerify.Verify(contentRaw, signatureRaw, publicKeyRaw);
                return isValid;
            }
            catch
            {
                return false;
            }
        }

        private void BtnVeriySignature_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = VerifySignature(txtSignature.Text, txtJsonFile.Text);
            this.shpIsValid.Fill = isValid ? Brushes.Green : Brushes.Red;
            lblVerifyResult.Content = isValid.ToString();

            if (isValid)
            {
                lblVerifyResult.Content = "Valid Signature";
                lblVerifyResult.Foreground = Brushes.Green;
            }
            else
            {
                lblVerifyResult.Content = "Invalid";
                lblVerifyResult.Foreground = Brushes.Red;
            }
        }

        private void BtnVerifySignatureTampered_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = VerifySignature(txtSignature.Text, txtJsonFileTampered.Text);
            this.shpIsValid.Fill = isValid ? Brushes.Green : Brushes.Red;
            lblVerifyAlternateResult.Content = isValid.ToString();

            if (isValid)
            {
                lblVerifyAlternateResult.Content = "Valid Signature";
                lblVerifyAlternateResult.Foreground = Brushes.Green;
            }
            else
            {
                lblVerifyAlternateResult.Content = "Invalid";
                lblVerifyAlternateResult.Foreground = Brushes.Red;
            }
        }

        private void BtnOpenCanonFileLocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(System.IO.Path.GetDirectoryName(txtCanonicalizedFile.Text));
            }
            catch
            {
            }

        }

        private void BtnOpenSampleFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("notepad.exe", txtJsonFile.Text);
            }
            catch
            {
            }
        }

        private void BtnOpenAlternateFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("notepad.exe", txtJsonFileTampered.Text);
            }
            catch
            {
            }
        }
    }
}
