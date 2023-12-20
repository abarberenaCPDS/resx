using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Veros.Crypto.Ecdsa.SignButVerify
{
    public interface ISigningKeyFactory
    {
        byte[] Sign(byte[] dataFileRaw);
    }

    public class SigningKey : ISigningKeyFactory
    {
        private readonly byte[] _privateKeyRaw;

        public SigningKey(byte[] privateKeyRaw)
        {
            this._privateKeyRaw = privateKeyRaw;
        }

        public byte[] Sign(byte[] dataFileRaw)
        {
            ISignButVerify signButVerify = new SignButVerify();
            byte[] signatureRaw = signButVerify.Sign(this._privateKeyRaw, dataFileRaw);
            return signatureRaw;
        }
    }

    public static class SigningKeyFactory
    {
        private static Dictionary<string, PrivateKeyRecord> _keysDictionary = new Dictionary<string, PrivateKeyRecord>();

        struct PrivateKeyRecord
        {
            public readonly string Key;
            public readonly byte[] SigningKeyRaw;
            public PrivateKeyRecord(string key, byte[] signingKeyRaw)
            {
                this.Key = key;
                this.SigningKeyRaw = signingKeyRaw;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static I CreateFor<C, I>(string privateKeyFileSerialNumber, string privateKeyFilePassword)
            where I : class
            where C : class, I
        {
            PrivateKeyRecord record = GetPrivateKeyRecord<C, I>(privateKeyFileSerialNumber, privateKeyFilePassword);
            ISigningKeyFactory factory = new SigningKey(record.SigningKeyRaw);
            return (I)factory;
        }

        private static PrivateKeyRecord GetPrivateKeyRecord<C, I>(string privateKeyFile, string privateKeyFilePassword)
            where C : class, I
            where I : class
        {
            PrivateKeyRecord privateKey;
            string uniqueKey = privateKeyFile;

            if (_keysDictionary.ContainsKey(uniqueKey))
            {
                privateKey = _keysDictionary[uniqueKey];
            }
            else
            {
                byte[] privateKeyRaw = GetPrivateKey(privateKeyFile, privateKeyFilePassword);
                privateKey = new PrivateKeyRecord(uniqueKey, privateKeyRaw);
                _keysDictionary.Add(uniqueKey, privateKey);
            }

            return privateKey;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static I CreateSigningKeyInstance<C, I>(string privateKeyFile, string privateKeyPassword)
            where I : class
            where C : class, I
        {
            ISignButVerify factory = new SignButVerify();
            factory.LoadPrivateKeyFile(privateKeyFile, privateKeyPassword);
            return (I)factory;
        }

        private static byte[] GetPrivateKey(string privateKeyFileSerialNumber, string privateKeyFilePassword)
        {
            ISignButVerify signButVerifyInstance = new SignButVerify();
            byte[] signingKeyRaw = signButVerifyInstance.LoadPrivateKeyFile(privateKeyFileSerialNumber, privateKeyFilePassword);
            return signingKeyRaw;
        }
    }
}