using Veros.Crypto.Ecdsa.SignButVerify;

namespace TestClient
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string privateKeyFile;
            string privateKeyFilePwd;
            string dataFileStr;
            byte[] dataFileRaw;
            byte[] signatureRaw;
            string signatureStr;
            byte[] signingKeyRaw;

            string publicKeyFile;
            byte[] publicKeyFileRaw;
            bool isValid;

            ISignButVerify iSign;
            ISigningKeyFactory signingKeyFactory;


            // arrange
            privateKeyFile = @"prv.pem";
            privateKeyFilePwd = string.Empty;
            string certificateSerialNumber = "3bb1f896d61260f39d1a5a8777349105b920beb4"; // "4b000001556aca5e33a0dc8a21000000000155";
            dataFileStr = @"sample.json";
            signatureStr = string.Empty;

            //using a PEM key
            publicKeyFile =
                "vselect-devqapub.pem"
                //"pub-from-pfx.pem"
                ;

            ////using a PFX key
            //publicKeyFile = "pfx-dev-pwd.p12";

            // act
            dataFileRaw = File.ReadAllBytes(dataFileStr);

            // using the signing key factory
            signingKeyFactory = SigningKeyFactory.CreateFor<SigningKey, ISigningKeyFactory>(certificateSerialNumber, privateKeyFilePwd);
            signatureRaw = signingKeyFactory.Sign(dataFileRaw);
            signatureStr = Convert.ToBase64String(signatureRaw);

            // V1
            //iSign = SigningKeyFactory.CreateFor<SignButVerify, ISignButVerify>(vendor);
            //signatureRaw = iSign.Sign(null, dataFileRaw);
            //signatureStr = Convert.ToBase64String(signatureRaw);

            // Native
            ISignButVerify signButVerify = new SignButVerify();
            //signatureRaw = signButVerify.Sign(signingKeyRaw, dataFileRaw);
            //signatureStr = Convert.ToBase64String(signatureRaw);

            //signingKeyRaw = SigningKeyFactory.Load(privateKeyFile, privateKeyFilePwd);
            //signatureRaw = signButVerify.Sign(signingKeyRaw, dataFileRaw);

            publicKeyFileRaw = signButVerify.LoadPublicKey(publicKeyFile);
            isValid = signButVerify.Verify(dataFileRaw, signatureRaw, publicKeyFileRaw);

            // assert
            Assert.IsNotNull(string.IsNullOrEmpty(signatureStr));
            Assert.IsTrue(isValid);

        }
    }
}