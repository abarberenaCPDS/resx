using System.Security.Cryptography;

namespace Veros.Crypto.Ecdsa.SignAndVerify
{
    public interface ISign
    {
        CngKey LoadPrivateKey(string content, string pwd);
        byte[] LoadPrivateKeyFile(string keyFile, string pwd);
        byte[] Sign(CngKey privateKey, byte[] data);
        byte[] Sign(byte[] privateKey, byte[] data);
    }
}