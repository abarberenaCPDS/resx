using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Veros.Crypto.Ecdsa.SignButVerify;

namespace EcdsaSignButVerify
{
    internal class SignButVerifyWinOnly : ISignButVerify
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

            using (ECDsa ecdsa = new ECDsaCng(privateKey))
            {
                signedRaw = ecdsa.SignData(data, HashAlgorithmName.SHA256, DSASignatureFormat.Rfc3279DerSequence);
            }
            return signedRaw;
        }

        public byte[] LoadPrivateKeyFile(string keyFile, string pwd)
        {
            byte[] result;
            //byte[] privateKeyRaw = File.ReadAllBytes(keyFile);
            var privateKeyRaw = File.ReadAllText(keyFile);
            using (ECDsa eCDsa = ECDsa.Create())
            {
                //eCDsa.ImportECPrivateKey(privateKeyRaw, out _);
                eCDsa.ImportFromPem(privateKeyRaw);
                result = eCDsa.ExportECPrivateKey();
            }
            return result;
        }

        public byte[] Sign(byte[] privateKeyRaw, byte[] data)
        {
            byte[] result;
            using (ECDsa eCDsa = ECDsa.Create())
            {
                eCDsa.ImportECPrivateKey(privateKeyRaw, out _);
                result = eCDsa.SignData(data, HashAlgorithmName.SHA256, DSASignatureFormat.Rfc3279DerSequence);
            }
            return result;
        }

        public byte[] LoadPublicKey(string pubKeyFile)
        {
            byte[] result;
            CngKey key;

            byte[] publicKeyRaw = File.ReadAllBytes(pubKeyFile);
            key = CngKey.Import(publicKeyRaw, CngKeyBlobFormat.EccFullPublicBlob);
            result = key.Export(CngKeyBlobFormat.EccPublicBlob);

            Debug.Assert(result != null);
            return result;
        }

        public bool Verify(byte[] contentRaw, byte[] signedRaw, byte[] publicKeyRaw)
        {
            bool result;
            CngKey publicKey = CngKey.Import(publicKeyRaw, CngKeyBlobFormat.EccFullPublicBlob);
            using (ECDsaCng ecsdKey = new ECDsaCng(publicKey))
            {
                result = ecsdKey.VerifyData(contentRaw, signedRaw, HashAlgorithmName.SHA256, DSASignatureFormat.Rfc3279DerSequence);
            }
            return result;
        }
    }
}