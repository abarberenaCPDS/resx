using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Veros.Crypto.Ecdsa.SignAndVerify;

namespace EcdsaSignButVerify
{
    internal class SignButVerify : ISignButVerify
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

        public bool Verify(byte[] contentRaw, byte[] signedRaw, CngKey publicKey)
        {
            bool result;
            using (ECDsaCng ecsdKey = new ECDsaCng(publicKey))
            {
                result = ecsdKey.VerifyData(contentRaw, signedRaw, HashAlgorithmName.SHA256, DSASignatureFormat.Rfc3279DerSequence);
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
    }
}
