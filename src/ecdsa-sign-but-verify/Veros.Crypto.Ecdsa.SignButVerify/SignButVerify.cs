using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.OpenSsl;

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
            byte[] signedRaw;
            string signedStr;

            var privateKeyString = File.ReadAllText(@"prv.pem");
            using (var textReader = new StringReader(privateKeyString))
            {
                // Only a private key
                var p = new PemReader(textReader).ReadPemObject();
                PrivateKeyInfo privateKeyInfo = PrivateKeyInfo.GetInstance(p.Content);
                ECPrivateKeyParameters privateKey1 = (ECPrivateKeyParameters)PrivateKeyFactory.CreateKey(privateKeyInfo);

                ISigner signer = SignerUtilities.GetSigner("SHA256withECDSA");
                signer.Init(true, privateKey1);
                signer.BlockUpdate(data,0, data.Length);
                signedRaw = signer.GenerateSignature();
                signedStr = Convert.ToBase64String(signedRaw);

                //pubRaw = privateKeyInfo.PublicKeyData.GetBytes();
                //pubStr = Convert.ToBase64String(pubRaw);
            }
            return signedRaw;
        }
    }
}