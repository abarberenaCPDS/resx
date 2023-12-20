namespace Veros.Crypto.Ecdsa.SignButVerify
{
    public interface ISign
    {
        /// <summary>
        /// Load the private key file
        /// </summary>
        /// <param name="keyFile"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        byte[] LoadPrivateKeyFile(string keyFile, string password);

        /// <summary>
        /// Sign data using the private key file
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] Sign(byte[] privateKey, byte[] data);
    }
}