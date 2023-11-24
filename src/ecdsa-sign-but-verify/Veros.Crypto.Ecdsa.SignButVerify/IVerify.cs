using System.Security.Cryptography;

namespace Veros.Crypto.Ecdsa.SignAndVerify
{
    public interface IVerify
    {
        CngKey LoadPublicKey(string pubKeyFile);

        bool Verify(byte[] contentRaw, byte[] signedRaw, CngKey publicKey);
    }
}