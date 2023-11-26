using System.Security.Cryptography;
using System.Text;
using Veros.Crypto.Ecdsa.SignAndVerify;

namespace EcdsaSignAndVerify;

class Program
{
    static void Main(string[] args)
    {
        // TODO: Test using the default file ...
        if (!args.Any())
            args = new[] { @"sample.json" };

        string content = args[0];

        // This method signs the data using the Private Key
        // and verifies the digital signing with the Public Key
        Test_SignAndVerify(content);

        // these are previously generated signatures
        // which are valid with the public key
        string signature =
            "MEYCIQD5vBVFMUwXKdpeXaEGcCBrYYooT4P/Rmoaz7nKl+2ovgIhAK4/kFWfc1U0dkTzD4IQxbSqs72s3Rufr+6fmctxfkRR"
            ;

        // This method just verifies the digital signing with the Public Key
        Test_VerifySign(content, signature);

        Console.WriteLine("\n\n+++ Program completed +++\n");
        Console.ReadLine();
    }

    private static void Test_VerifySign(string content, string signature)
    {
        Console.WriteLine("\n\n=== ECDSA - Just Verify Signature ===\n");

        Console.Write($"Signature:");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"\t\t{signature}\n");
        Console.ResetColor();

        byte[] contentRaw = File.ReadAllBytes(content);

        bool valid;
        string publicKeyFile = @"pub8.der"; // Windows 
        byte[] publicKeyRaw = File.ReadAllBytes(publicKeyFile);
        string publicKeyStr = Convert.ToBase64String(publicKeyRaw);
        Console.WriteLine($"Public Key:\t\t{publicKeyStr}");

        byte[] signatureRaw = Convert.FromBase64String(signature);

        // TODO: Verify public key format later
        //IVerify verify = new SignButVerify();
        //ECDsa eCDsa = LoadPublicKey(pub);
        //byte[] puk = eCDsa.ExportSubjectPublicKeyInfo();
        //CngKey pukCng = CngKey.Import(puk, CngKeyBlobFormat.EccFullPublicBlob);
        //verify.Verify(data, rawSignature, pukCng);

        // verify signature
        // Public Key loading using ECDSA, not ECDSACng
        valid = Verify(contentRaw, signatureRaw, publicKeyRaw);

        Console.Write($"\nIs Signature valid?\t\t");
        Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
        Console.Write($"{valid.ToString().ToUpper()}\t\t{content}"); Console.ResetColor();
    }

    private static void Test_SignAndVerify(string content)
    {
        Console.WriteLine("\n=== ECDSA - Sign but Verify ===\n");
        byte[] contentRaw = File.ReadAllBytes(content);
        Console.WriteLine($"Content:\t\t{content}");

        string privateKeyFile = @"prv8.der";
        string pwd =
            "P@ssw0rd"
        ;

        ISign signButVerify = new EcdsaSignButVerify.SignButVerify();

        /// Sign data....
        Console.WriteLine($"Private key file:\t{privateKeyFile}");
        CngKey privateKey = signButVerify.LoadPrivateKey(privateKeyFile, pwd);
        Console.WriteLine($"Private Key loaded:\t{privateKey.Algorithm} ready.");

        byte[] signatureRaw = signButVerify.Sign(privateKey, contentRaw);
        string signatureStr = Convert.ToBase64String(signatureRaw);
        Console.Write($"Signature:");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"\t\t{signatureStr}\n");
        Console.ResetColor();

        bool valid;
        string pubKeyFile = @"pub8.pem";
        byte[] publicKeyRaw = File.ReadAllBytes(pubKeyFile);
        string publicKeyStr = Convert.ToBase64String(publicKeyRaw);
        Console.WriteLine($"Public Key:\t\t{publicKeyStr}");

        // verify signature
        // Public Key loading using ECDSA, not ECDSACng

        // TODO: Refactor the public key to byte array...
        //IVerify justVerify = new EcdsaSignButVerify.SignButVerify();
        //CngKey pubKey = justVerify.LoadPublicKey(pubKeyFile);
        //justVerify.Verify(contentRaw, signatureRaw, null);

        valid = Verify(contentRaw, signatureRaw, publicKeyRaw);

        Console.Write($"\nIs Signature valid?\t\t");
        Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
        Console.Write($"{valid.ToString().ToUpper()}\t\t{content}"); Console.ResetColor();

        // vverify signature for tampered payload
        content = @"sample-tampered.json";
        contentRaw = File.ReadAllBytes(content);

        valid = Verify(contentRaw, signatureRaw, publicKeyRaw);

        Console.Write($"\nIs Signature valid?\t\t");
        Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
        Console.Write($"{valid.ToString().ToUpper()}\t\t{content}"); Console.ResetColor();
    }

    private static bool Verify(byte[] msg, byte[] signature, byte[] pubKey)
    {
        bool result;

        using (ECDsa eCDsa = ECDsa.Create())
        {
            eCDsa.ImportSubjectPublicKeyInfo(pubKey, out _);
            result = eCDsa.VerifyData(msg, signature, HashAlgorithmName.SHA256, DSASignatureFormat.Rfc3279DerSequence);
        }
        return result;
    }
}