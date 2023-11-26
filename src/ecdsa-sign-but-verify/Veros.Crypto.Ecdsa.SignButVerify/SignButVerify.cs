using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Veros.Crypto.Ecdsa.SignAndVerify
{
    public class SignButVerify : ISignButVerify
    {
        public CngKey LoadPrivateKey(string content, string pwd)
        {
            CngKey result;

            var privateKeyRaw = File.ReadAllBytes(content);
            byte[] pwdPrivateKeyRaw = Encoding.UTF8.GetBytes(pwd);

            result = CngKey.Import(privateKeyRaw, CngKeyBlobFormat.Pkcs8PrivateBlob);
            Debug.Assert(result != null);
            return result;
        }

        public byte[] Sign(CngKey privateKey, byte[] data)
        {
            byte[] signedRaw;
            byte[] pubRaw;
            //string signed;

            using (ECDsaCng ecdsa = new ECDsaCng(privateKey))
            {
                signedRaw = ecdsa.SignData(data);
                //signed = Convert.ToBase64String(signedRaw);
                //pubRaw = privateKey.Export(CngKeyBlobFormat.EccPublicBlob);
                //Console.WriteLine(Convert.ToBase64String(pubRaw));
            }
            return signedRaw;
        }

        public bool Verify(byte[] contentRaw, byte[] signedRaw, CngKey publicKey)
        {
            bool result;
            using (ECDsaCng ecsdKey = new ECDsaCng(publicKey))
            {
                result = ecsdKey.VerifyData(contentRaw, signedRaw);
            }
            return result;
        }

        public CngKey LoadPublicKey(string pubKeyFile)
        {
            CngKey result;

            byte[] publicKeyRaw = File.ReadAllBytes(pubKeyFile);
            result = CngKey.Import(publicKeyRaw, CngKeyBlobFormat.EccFullPublicBlob);

            Debug.Assert(result != null);
            return result;
        }

        public byte[] LoadPrivateKeyFile(string keyFile, string pwd)
        {
            throw new System.NotImplementedException();
        }

        public byte[] Sign(byte[] privateKey, byte[] data)
        {
            throw new System.NotImplementedException();
        }
    }
}