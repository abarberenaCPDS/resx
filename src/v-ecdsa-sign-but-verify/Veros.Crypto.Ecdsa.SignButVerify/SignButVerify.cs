using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Veros.Crypto.Ecdsa.SignButVerify
{
    public class SignButVerify : ISignButVerify
    {

        public byte[] LoadPrivateKeyFile(string keyFile, string keyFilePassword)
        {
            byte[] result;
            FileInfo fi = new FileInfo(keyFile);

            switch (fi.Extension.ToLower())
            {
                case ".pfx":
                case ".p12":
                    result = this.LoadPrivateKeyFileFromPFX(keyFile, keyFilePassword);
                    break;
                case ".pem":
                    result = this.LoadPrivateKeyFileFromPEM(keyFile, keyFilePassword);
                    break;
                default:
                    result = LoadPrivateKeyFromWindowsCertStore(keyFile);
                    break;
            }

            return result;
        }

        private byte[] LoadPrivateKeyFileFromPEM(string keyFile, string keyFilePassword)
        {
            byte[] result;
            var privateKeyContent = File.ReadAllText(keyFile);
            using (var textReader = new StringReader(privateKeyContent))
            {
                var privateKeyPem = new PemReader(textReader).ReadPemObject();
                result = privateKeyPem.Content;
            }
            return result;
        }

        private byte[] LoadPrivateKeyFileFromPFX(string keyFile, string keyFilePassword)
        {
            byte[] result;

            Pkcs12Store pkcs12Store;

            using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(keyFile)))
            {
                result = ms.ToArray();
                pkcs12Store = new Pkcs12Store(ms, keyFilePassword.ToCharArray());
            }

            string certificateFriendlyName = pkcs12Store.Aliases.Cast<string>().FirstOrDefault();
            AsymmetricKeyEntry assymetricKeyParameter = pkcs12Store.GetKey(certificateFriendlyName);

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(assymetricKeyParameter.Key);

            result = privateKeyInfo.GetEncoded();
            //ECPrivateKeyParameters privateKeyPrams = assymetricKeyParameter.Key as ECPrivateKeyParameters;
            return result;
        }

        private byte[] LoadPrivateKeyFromWindowsCertStore(string certificateSerialNumber)
        {
            byte[] result;
            StoreName certificateStoreName = StoreName.My;
            X509Certificate2Collection certitificatesCollection;

            StoreLocation certificateStoreLocation = StoreLocation.CurrentUser;
            //StoreLocation certificateStoreLocation = StoreLocation.LocalMachine;

            // find certificate
            using (X509Store certStore = new X509Store(certificateStoreName, certificateStoreLocation))
            {
                certStore.Open(OpenFlags.ReadOnly);
                certitificatesCollection = certStore.Certificates.Find(X509FindType.FindBySerialNumber, certificateSerialNumber, false);
            }

            X509Certificate2 signingCertificate = certitificatesCollection.OfType<X509Certificate2>().FirstOrDefault();
            if (signingCertificate == null)
            {
                throw new ArgumentNullException("Signing Certificate", $"The certificate SN: {certificateSerialNumber} was not found in Store: {certificateStoreLocation}/{certificateStoreName}.");
            }

            Pkcs12Store pkcs12Store;

            // parse the password-protected certificate to PKCS#12 format
            byte[] signingCertificateRaw;
            try
            {
                signingCertificateRaw = signingCertificate.Export(X509ContentType.Pkcs12, certificateSerialNumber);
            }
            catch (CryptographicException ex)
            {
                throw new ArgumentException($"Signing Certificate SN: {certificateSerialNumber}", ex.Message);
            }

            using (MemoryStream ms = new MemoryStream(signingCertificateRaw, false))
            {
                pkcs12Store = new Pkcs12Store(ms, certificateSerialNumber.ToCharArray());
            }

            // retrieve the private key from PKCS12 format
            string certificateFriendlyName = pkcs12Store.Aliases.Cast<string>().FirstOrDefault(x => pkcs12Store.IsKeyEntry(x) && pkcs12Store.GetKey(x).Key.IsPrivate);
            if (string.IsNullOrEmpty(certificateFriendlyName))
            {
                throw new ArgumentNullException($"The certificate SN: {certificateSerialNumber} does not contain a private key");
            }

            AsymmetricKeyEntry assymetricKeyEntry = pkcs12Store.GetKey(certificateFriendlyName);

            //Debug.Assert(assymetricKeyEntry != null); //prove me wrong...
            if (assymetricKeyEntry == null)
            {
                throw new ArgumentNullException("Private Key", $"The private key could not be found for certificate SN: {certificateSerialNumber}. Friendly name: {certificateFriendlyName}.");
            }

            // parse the private key to standard format
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(assymetricKeyEntry.Key);
            result = privateKeyInfo.GetEncoded();

            return result;
        }

        public byte[] LoadPublicKey(string pubKeyFile)
        {
            byte[] result;
            FileInfo fi = new FileInfo(pubKeyFile);

            switch (fi.Extension.ToLower())
            {
                case ".pfx":
                case ".p12":
                    result = this.LoadPublicKeyFileFromPFX(pubKeyFile);
                    break;
                default:
                    result = this.LoadPublicKeyFileFromPEM(pubKeyFile);
                    break;
            }
            return result;
        }

        private byte[] LoadPublicKeyFileFromPEM(string pubKeyFile)
        {
            byte[] result;
            var publicKeyContent = File.ReadAllText(pubKeyFile);
            using (var textReader = new StringReader(publicKeyContent))
            {
                var publicKeyPem = new PemReader(textReader).ReadPemObject();
                result = publicKeyPem.Content;
            }
            return result;
        }

        private byte[] LoadPublicKeyFileFromPFX(string pubKeyFile)
        {
            throw new NotImplementedException();
        }

        public byte[] Sign(byte[] privateKeyRaw, byte[] data)
        {
            byte[] result;

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfo.GetInstance(privateKeyRaw);
            ECPrivateKeyParameters privateKeyParam = PrivateKeyFactory.CreateKey(privateKeyInfo) as ECPrivateKeyParameters;

            ISigner signer = SignerUtilities.GetSigner("SHA256withECDSA");
            signer.Init(true, privateKeyParam);
            signer.BlockUpdate(data, 0, data.Length);
            result = signer.GenerateSignature();

            return result;
        }

        public bool Verify(byte[] contentRaw, byte[] signedRaw, byte[] publicKeyRaw)
        {
            bool result;

            SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfo.GetInstance(publicKeyRaw);
            ECPublicKeyParameters publicKeyParam = PublicKeyFactory.CreateKey(subjectPublicKeyInfo) as ECPublicKeyParameters;

            ISigner signer = SignerUtilities.GetSigner("SHA256withECDSA");
            signer.Init(false, publicKeyParam);
            signer.BlockUpdate(contentRaw, 0, contentRaw.Length);
            result = signer.VerifySignature(signedRaw);

            return result;
        }

    }
}