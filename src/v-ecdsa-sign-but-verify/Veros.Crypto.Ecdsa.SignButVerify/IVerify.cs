namespace Veros.Crypto.Ecdsa.SignButVerify
{
    public interface IVerify
    {
        byte[] LoadPublicKey(string pubKeyFile);

        bool Verify(byte[] contentRaw, byte[] signedRaw, byte[] publicKeyRaw);
    }
}