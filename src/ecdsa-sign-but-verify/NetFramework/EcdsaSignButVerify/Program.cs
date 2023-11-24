using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Veros.Crypto.Ecdsa.SignAndVerify;

namespace NetEcdsaSignButVerify
{
    internal class Program
    {
        private static void Test_VerifySign(string content, string signature)
        {
            // HACK: REMOVE this later...

            //Console.WriteLine("\n\n=== ECDSA - Just Verify Signature ===\n");
            //Console.WriteLine($"Signature: {signature}");

            //byte[] data = File.ReadAllBytes(content);

            //bool valid;
            //string pubKeyFile = @"pub.der";
            //byte[] pub = File.ReadAllBytes(pubKeyFile);
            //Console.WriteLine($"Public Key: {Convert.ToBase64String(pub)}");

            //byte[] rawSignature =
            //     Convert.FromBase64String(signature)
            //// signatureBytes
            //;

            //// valid payload
            //SignAndVerify signAndVerify = new SignAndVerify();
            //valid = signAndVerify.Verify(data, rawSignature, pub);

            //Console.Write($"\nIs Valid? --> ");
            //Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
            //Console.Write($"{valid.ToString().ToUpper()}"); Console.ResetColor();
        }

        private static void Test_SignAndVerify(string content)
        {
            Console.WriteLine("\n=== ECDSA - Sign but Verify ===\n");
            byte[] contentRaw = File.ReadAllBytes(content);
            Console.WriteLine($"Content:\t\t\t{content} loaded...");

            byte[] signedRaw;
            string signedStr;

            string privateKeyFile = @"prv8.der";
            string pwd =
                "P@ssw0rd"
            ;

            ISignButVerify signButVerify = new SignButVerify();

            /// Sign data....
            Console.WriteLine($"Private key file:\t\t{privateKeyFile}");
            CngKey privateKey = signButVerify.LoadPrivateKey(privateKeyFile, pwd);
            Console.WriteLine($"OK: Private Key loaded:\t\t{privateKey.Algorithm} ready.");

            signedRaw = signButVerify.Sign(privateKey, contentRaw);
            signedStr = Convert.ToBase64String(signedRaw);
            Console.WriteLine($"Signature:\t\t\t{signedStr}");
            Console.ReadLine();
            return;

            // HACK: This does not work, does not load the public key

            /// Verify data...
            bool valid;
            //string pubKeyFile = @"pub.pem";
            //string pubKeyFile = @"pub.der";
            //string pubKeyFile = @"pub8.pem";
            string pubKeyFile = @"pub8.der";

            CngKey publicKey = signButVerify.LoadPublicKey(pubKeyFile);
            //string publicKeyStr = Convert.ToBase64String(publicKey.);
            Console.WriteLine($"Public Key: {publicKey.Algorithm}");

            //signedRaw = "";
            //signedStr = Convert.ToBase64String(signedRaw);

            // valid payload
            valid = signButVerify.Verify(contentRaw, signedRaw, publicKey);

            Console.Write($"\nIs Valid? --> ");
            Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{valid.ToString().ToUpper()}"); Console.ResetColor();

            // tampered payload
            content = @"sample-tampered.json";
            contentRaw = File.ReadAllBytes(content);

            valid = signButVerify.Verify(contentRaw, signedRaw, publicKey);

            Console.Write($"\nIs Valid? --> ");
            Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{valid.ToString().ToUpper()}"); Console.ResetColor();
        }

        static void Main(string[] args)
        {
            // Original

            // TODO: Test using the default file ...
            if (!args.Any())
                args = new[] { @"sample.json" };

            string content = args[0];
            Test_SignAndVerify(content);

            return;


            // sample of valid 
            string signature =
                "qV7sn7JHq2sbwDGtS2WV1OmCcBHHm8ZvCtjSWoTCdCHSE6ysNXbxjreGDWPR1Sy6i2Jeaj1L985+72XWFeUnEQ=="
                //"VQ9qkPTyYBQUN3Q2czC2RZox4BT4bTAegibYnv5c4ajlFnxMFsY53AC7650zY9AtyzhVWXy/cI2XaVkt2kLq4g=="
                ;
            Test_VerifySign(content, signature);

            Console.WriteLine("\n\n+++ Program completed +++\n");
        }
    }
}
