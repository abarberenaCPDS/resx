using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NetEcdsaSignButVerify
{
    public static class SignAndVerify
    {
        internal static ECDsaCng LoadPublicKey(byte[] pubKey)
        {
            //ECParameters ecParams = new ECParameters();
            //ecParams.Curve = ECCurve.NamedCurves.nistP256;

            //if (pubKey.Length%2==1)// && pubKey[0]==0x04)
            //{
            //    byte[] qx = new byte[pubKey.Length / 2];
            //    byte[] qy = new byte[qx.Length];
            //    Buffer.BlockCopy(pubKey,1,qx,0,qx.Length);
            //    Buffer.BlockCopy(pubKey,1+ qx.Length, qy,0,qy.Length);

            //    ecParams.Q = new ECPoint { X = qx, Y=qy };
            //}

            //using (ECDiffieHellman k = ECDiffieHellman.Create(ecParams))
            //{
            //    var r = k.PublicKey;
            //}
            CngKey k = CngKey.Import(pubKey, CngKeyBlobFormat.EccPublicBlob);
            ECDsaCng cng = new ECDsaCng(k);

            ECDsa eCDsa = ECDsa.Create();
            //eCDsa.ImportSubjectPublicKeyInfo(pubKey, out _);
            return null;
        }

        internal static ECDsa LoadPrivateKey(string content, string pwd)
        {
            var priv = File.ReadAllBytes(content);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(pwd);

            CngKey k = CngKey.Import(priv, CngKeyBlobFormat.EccPrivateBlob);
            ECDsaCng cng = new ECDsaCng(k);

            ECDsa eCDsa = ECDsa.Create();

            //eCDsa.ImportEncryptedPkcs8PrivateKey(pwdBytes, priv, out _);

            // TODO: refactor this later...
            // PbeParameters pbeParameters =
            //     new PbeParameters(PbeEncryptionAlgorithm.Aes128Cbc, HashAlgorithmName.SHA256, 2048);
            // var exportedKey = eCDsa.ExportEncryptedPkcs8PrivateKey(pwd, pbeParameters);

            Console.WriteLine($"OK: Private Key loaded...: {eCDsa.SignatureAlgorithm} ready.");
            return eCDsa;
        }

        internal static byte[] Sign(byte[] msg, ECDsa eCDsa)
        {
            byte[] result = eCDsa.SignData(msg, HashAlgorithmName.SHA256);
            return result;
        }

        internal static bool Verify(byte[] msg, byte[] signature, byte[] pubKey)
        {
            bool result;

            ECDsa eCDsa = LoadPublicKey(pubKey);
            result = eCDsa.VerifyData(msg, signature, HashAlgorithmName.SHA256);
            return result;
        }
    }
}
