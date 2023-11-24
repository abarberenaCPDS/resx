using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NetEcdsaSignButVerify
{
    public static class SignAndVerify
    {
        internal static ECDsa LoadPublicKey(byte[] pubKey)
        {
            ECDsa eCDsa = ECDsa.Create();
            eCDsa.ImportSubjectPublicKeyInfo(pubKey, out _);
            return eCDsa;
        }

        internal static ECDsa LoadPrivateKey(string content, string pwd)
        {
            var priv = File.ReadAllBytes(content);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(pwd);

            ECDsa eCDsa = ECDsa.Create();
            eCDsa.ImportEncryptedPkcs8PrivateKey(pwdBytes, priv, out _);

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
