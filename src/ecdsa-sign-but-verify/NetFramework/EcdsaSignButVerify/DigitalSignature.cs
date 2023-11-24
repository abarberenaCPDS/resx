using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NetEcdsaSignButVerify
{
    public class DigitalSignature
    {
        public string prvKeyStr { get; set; }
        public string pubKeyStr { get; set; }

        public byte[] prvKeyRaw { get; set; }
        public byte[] pubKeyRaw { get; set; }


        public void ShowKeys()
        {
            Console.WriteLine("\n\n===========================================\n");
            Console.WriteLine($"Private Key: {Convert.ToBase64String(prvKeyRaw)}");
            Console.WriteLine($"Public Key : {Convert.ToBase64String(pubKeyRaw)}");
            Console.WriteLine("\n===========================================\n");
        }

        private ECDsa LoadPrivateKey(string pem, string pemPwd)
        {
            ECDsa result = null;
            byte[] pemRaw = File.ReadAllBytes(pem);
            string pemStr;

            byte[] pwdRaw = Encoding.UTF8.GetBytes(pemPwd);

            //pemRaw = this.prvKeyRaw;

            byte[] signedRaw;
            string signed;

            CngKey loadedPrivKey = CngKey.Import(pemRaw, CngKeyBlobFormat.Pkcs8PrivateBlob);
            using (ECDsaCng ecdsa = new ECDsaCng(loadedPrivKey))
            {
                pemStr = Convert.ToBase64String(pemRaw);
                signedRaw = ecdsa.SignData(Encoding.UTF8.GetBytes("abe"));
                signed = Convert.ToBase64String(signedRaw);
                result = ecdsa;
            }
            return result;
        }

        private byte[] LoadPublicKey(string pem)
        {
            byte[] result = null;
            return result;
        }

        public void Run()
        {
            //this.LoadPrivateKey(@"prv2-clean.pem", string.Empty);
            ECDsa ecdsaPrivateKey = this.LoadPrivateKey(@"prv8.der", string.Empty);
            //this.LoadPrivateKey(@"prv2.der", string.Empty);
            //this.LoadPrivateKey(@"prv2.pem", string.Empty);
            this.LoadPublicKey(@"pub2.pem");


            string signed = "Cjd+umtLlj9dYNUIiy8ETuq8xFJQE4dLRX6owuRrCue2XEUMjE7dgUljUuluJvZi9SvQh+XGfvDhEU5kbvYbGA==";
        }


        public void SelfGenerated()
        {
            using (ECDsaCng ecdsa = new ECDsaCng(ECCurve.NamedCurves.nistP256))
            {
                ecdsa.HashAlgorithm = CngAlgorithm.Sha256;
                // calculate Priv & Pub keys
                prvKeyRaw = ecdsa.Key.Export(CngKeyBlobFormat.Pkcs8PrivateBlob);
                prvKeyStr = Convert.ToBase64String(prvKeyRaw);

                pubKeyRaw = ecdsa.Key.Export(CngKeyBlobFormat.GenericPublicBlob);
                pubKeyStr = Convert.ToBase64String(pubKeyRaw);

                this.ShowKeys();
            }

            CngKey pubKey = CngKey.Import(this.pubKeyRaw, CngKeyBlobFormat.EccPublicBlob);
            using (ECDsaCng ecdsa = new ECDsaCng(pubKey))
            {
                string s = "";
            }

        }

        public void B()
        {
            // original implementation
            Bob bob = new Bob();
            using (ECDsaCng dsa = new ECDsaCng())
            {
                bob.key = prvKeyRaw;

                bob.pubKey = dsa.Key.Export(CngKeyBlobFormat.EccPublicBlob);
                //bob.key = dsa.Key.Export(CngKeyBlobFormat.EccPublicBlob);

                byte[] data = new byte[] { 21, 5, 8, 12, 207 };

                byte[] signature = dsa.SignData(data);
                bob.Receive(data, signature);
            }
        }
    }

    public class Bob
    {
        public byte[] key;
        public byte[] privKey;
        public byte[] pubKey;

        public void Receive(byte[] data, byte[] signature)
        {
            using (ECDsaCng ecsdKey = new ECDsaCng(CngKey.Import(key, CngKeyBlobFormat.EccPublicBlob)))
            {
                if (ecsdKey.VerifyData(data, signature))
                    Console.WriteLine("Data is good");
                else
                    Console.WriteLine("Data is bad");
            }
        }
    }
}
