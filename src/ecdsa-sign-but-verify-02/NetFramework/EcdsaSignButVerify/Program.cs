using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace NetEcdsaSignButVerify
{
    internal class Program
    {
        private static void Test_VerifySign(string content, string signature)
        {
            Console.WriteLine("\n\n=== ECDSA - Just Verify Signature ===\n");
            Console.WriteLine($"Signature: {signature}");

            byte[] data = File.ReadAllBytes(content);

            bool valid;
            string pubKeyFile = @"pub.der";
            byte[] pub = File.ReadAllBytes(pubKeyFile);
            Console.WriteLine($"Public Key: {Convert.ToBase64String(pub)}");

            byte[] rawSignature =
                 Convert.FromBase64String(signature)
            // signatureBytes
            ;

            // valid payload
            valid = SignAndVerify.Verify(data, rawSignature, pub);

            Console.Write($"\nIs Valid? --> ");
            Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{valid.ToString().ToUpper()}"); Console.ResetColor();
        }

        private static void Test_SignAndVerify(string content)
        {
            Console.WriteLine("\n=== ECDSA - Sign but Verify ===\n");
            // string content = @"sample.json";
            byte[] data = File.ReadAllBytes(content);

            string privateKeyFile = @"prv.der";
            string pwd =
                "P@ssw0rd"
            ;

            ECDsa eCDsa = SignAndVerify.LoadPrivateKey(privateKeyFile, pwd);
            byte[] signed = SignAndVerify.Sign(data, eCDsa);
            //signatureBytes = signed;
            //signatureString = Convert.ToBase64String(signed);
            Console.WriteLine($"Signature: {Convert.ToBase64String(signed)}");

            bool valid;
            string pubKeyFile = @"pub.der";
            byte[] pub = File.ReadAllBytes(pubKeyFile);
            Console.WriteLine($"Public Key: {Convert.ToBase64String(pub)}");

            // valid payload
            valid = SignAndVerify.Verify(data, signed, pub);

            Console.Write($"\nIs Valid? --> ");
            Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{valid.ToString().ToUpper()}"); Console.ResetColor();

            // tampered payload
            content = @"sample-tampered.json";
            data = File.ReadAllBytes(content);

            valid = SignAndVerify.Verify(data, signed, pub);

            Console.Write($"\nIs Valid? --> ");
            Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{valid.ToString().ToUpper()}"); Console.ResetColor();
        }

        static void Main(string[] args)
        {
            // TODO: Test using the default file ...
            if (!args.Any())
                args = new[] { @"sample.json" };

            string content = args[0];
            //Test_SignAndVerify(content);

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
