using System.Security.Cryptography;
using System.Text;
using Veros.Crypto.Ecdsa.SignButVerify;

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
        //string privateKeyFile = @"prv8.der";
        string privateKeyFile = @"prv.pem";
        string privateKeyFilePwd = "P@ssw0rd";
        string publicKeyFile = @"pub8.der"; // Windows DER format

        Test_SignAndVerify(content, privateKeyFile, privateKeyFilePwd, publicKeyFile);

        // these are previously generated signatures
        // which are valid with the public key
        string signature =
            "MEUCIQCfDqi2bvlYHEQmdMp7Tt6ZSUae6LeAmj1sPugnQdiKZwIgI4NzAf8Cg9Hwr07LWtuV2r2ZlWEzHZajpPw+uT/PBtI="
            ;

        // This method just verifies the digital signing with the Public Key
        Test_JustVerify(content, signature, publicKeyFile);

        Console.WriteLine("\n\n+++ Program completed +++\n");
        Console.ReadLine();
    }

    private static void Test_JustVerify(string content, string signature, string publicKeyFile)
    {
        Console.WriteLine("\n\n=== ECDSA - Just Verify Signature ===\n");

        Console.Write($"Signature:\t\t");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"{signature}\n");
        Console.ResetColor();

        byte[] contentRaw = File.ReadAllBytes(content);

        bool valid;

        Console.WriteLine($"Public KeyFile:\t\t{publicKeyFile}");
        byte[] publicKeyRaw = File.ReadAllBytes(publicKeyFile);
        string publicKeyStr = Convert.ToBase64String(publicKeyRaw);
        Console.WriteLine($"Public Key:\t\t{publicKeyStr}");

        byte[] signatureRaw = Convert.FromBase64String(signature);

        // Verify signature from DotNet Core using native ECDSA
        Console.WriteLine("\n\n=== Verify signature from DotNet Core === ");
        valid = Verify(contentRaw, signatureRaw, publicKeyRaw);

        Console.Write($"\nIs Signature valid?\t");
        Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
        Console.Write($"{valid.ToString().ToUpper()}\t\t{content}"); Console.ResetColor();
        Console.WriteLine();

        // Verify signature from .NET Framework
        Console.WriteLine("\n\n=== Verify signature from .NET Framework ===");

        IVerify signButVerify = new SignButVerify();
        publicKeyFile = "pub8.pem";
        publicKeyRaw = signButVerify.LoadPublicKey(publicKeyFile);
        valid = signButVerify.Verify(contentRaw, signatureRaw, publicKeyRaw);

        //valid = Verify(contentRaw, signatureRaw, publicKeyRaw);

        Console.Write($"\nIs Signature valid?\t");
        Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
        Console.Write($"{valid.ToString().ToUpper()}\t\t{content}"); Console.ResetColor();
        Console.WriteLine();
    }

    private static void Test_SignAndVerify(string content, string privateKeyFile, string privateKeyFilePwd, string publicKeyFile)
    {
        byte[] signatureRaw;
        string signatureStr;

        Console.WriteLine("\n=== ECDSA - Sign but Verify ===\n");
        byte[] contentRaw = File.ReadAllBytes(content);
        Console.WriteLine($"Content:\t\t{content}");

        /// Sign data....
        Console.WriteLine($"Private key file:\t{privateKeyFile}");

        //ISign signButVerify = new EcdsaSignButVerify.SignButVerify(); // EcdsaCng
        //CngKey privateKey = signButVerify.LoadPrivateKey(privateKeyFile, pwd);
        //Console.WriteLine($"Private Key loaded:\t{privateKey.Algorithm} ready.");

        ISign signButVerify = new SignButVerify();
        byte[] privateKey = signButVerify.LoadPrivateKeyFile(privateKeyFile, privateKeyFilePwd);

        signatureRaw = signButVerify.Sign(privateKey, contentRaw);
        signatureStr = Convert.ToBase64String(signatureRaw);

        Console.Write($"Signature:");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"\t\t{signatureStr}\n");
        Console.ResetColor();

        bool valid;

        byte[] publicKeyRaw = File.ReadAllBytes(publicKeyFile);
        string publicKeyStr = Convert.ToBase64String(publicKeyRaw);
        Console.WriteLine($"Public Key:\t\t{publicKeyStr}");

        // verify auto generated signature
        // Using DotNet Core ECDSA
        valid = Verify(contentRaw, signatureRaw, publicKeyRaw);

        Console.Write($"\nIs Signature valid?\t");
        Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
        Console.Write($"{valid.ToString().ToUpper()}\t\t{content}"); Console.ResetColor();

        // TAMPERED: verify signature for payload
        content = @"sample-tampered.json";
        contentRaw = File.ReadAllBytes(content);

        valid = Verify(contentRaw, signatureRaw, publicKeyRaw);

        Console.Write($"\nIs Signature valid?\t");
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