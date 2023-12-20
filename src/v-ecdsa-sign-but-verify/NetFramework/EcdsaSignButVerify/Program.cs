using System;
using System.IO;
using System.Linq;
using Veros.Crypto.Ecdsa.SignButVerify;

namespace NetEcdsaSignButVerify
{
    internal class Program
    {
        private static void Test_VerifySign(string content, string signature, string pubKeyFile)
        {
            Console.WriteLine("\n\n=== ECDSA - Just Verify Signature ===\n");
            Console.WriteLine($"Signature:\t{signature}");
            Console.WriteLine($"Public Key File:\t{pubKeyFile}");

            byte[] contentRaw = File.ReadAllBytes(content);

            bool isValid;
            byte[] pubKeyRaw;
            byte[] signatureRaw = Convert.FromBase64String(signature);

            IVerify signButVerify = new SignButVerify();
            pubKeyRaw = signButVerify.LoadPublicKey(pubKeyFile);
            isValid = signButVerify.Verify(contentRaw, signatureRaw, pubKeyRaw);

            Console.WriteLine($"Public Key:\t{Convert.ToBase64String(pubKeyRaw)}");

            Console.Write($"\nIs Valid? --> ");
            Console.ForegroundColor = isValid ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{isValid.ToString().ToUpper()}"); Console.ResetColor();
        }

        private static void Test_SignAndVerify(string content, string privateKeyFile, string privateKeyFilePwd)
        {
            Console.WriteLine("\n=== ECDSA - Sign but Verify ===\n");
            byte[] contentRaw = File.ReadAllBytes(content);
            Console.WriteLine($"Content:\t\t\t{content} loaded...");

            byte[] signedRaw;
            string signedStr;

            byte[] privateKeyFileRaw;

            /// Sign data....
            Console.WriteLine($"Private key file:\t\t{privateKeyFile}");

            ISignButVerify signButVerify = new SignButVerify();
            privateKeyFileRaw = signButVerify.LoadPrivateKeyFile(privateKeyFile, privateKeyFilePwd);
            signedRaw = signButVerify.Sign(privateKeyFileRaw, contentRaw);

            signedStr = Convert.ToBase64String(signedRaw);
            Console.WriteLine($"Signature:\t\t\t{signedStr}");
        }

        static void Main(string[] args)
        {
            // Use sample.json as default content file
            if (!args.Any())
                args = new[] { @"sample.json" };

            string content = args[0];

            // set the private key file and its password (if any/required)
            string privateKeyFile = @"prv.pem";
            string privateKeyFilePwd = "P@ssw0rd";

            Test_SignAndVerify(content, privateKeyFile, privateKeyFilePwd);

            //return;

            // Previouly generated valid signature
            string signature =
                "MEUCIGibIRxPTEIipIN+H5cfVkx2HYi5A4yY+dSApGA8wapvAiEAifBeNrRW7I2Bw6sX1cQ535fCRZQHJAzFUGUsi/Ep9Nk="
                //"MEQCIFSi2y//fR6uYVzZk34kMJAlDDzzMOTDjEo0lEdHTVliAiAVv+XH9eiDUOaovaQKqojyPSJCWOETdtwywmCnOk4cIA=="
                ;
            string pubKeyFile = @"pub.pem";

            Test_VerifySign(content, signature, pubKeyFile);

            Console.WriteLine("\n\n+++ Program completed +++\n");
            Console.ReadLine();
        }
    }
}
