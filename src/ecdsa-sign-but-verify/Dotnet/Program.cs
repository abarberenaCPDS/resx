using System;
using System.Diagnostics;
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
            "miyXZ6sf+IF2mdvMhOTE4A/WzMyTW4AQbk9FOOEobpryh9UY1VuqapA2sGn0Jov5cLMv66DksO+gH/RJQP2f6A=="
            //"VL59mMVloXhU7yA7jkE0/lk2SUonpuzT/3/tzoyDP1yPVNagUe43tyktZLqG/i3crIidcZF6h6nVSJF7u2V/DQ=="
            //"qrLFXYLveaOtvL2K28lVrcOfwj6Kvmixykb6ZcJh4WNUDRiObyWZVC7c2rOLZRQyqBQC9P4k4/cgibTkbJhUIQ=="
            //"NXJUpIQAxKjAVSt0YzoCFG8zgY2TZCRxs2dwacIWhIvRBBUgq44Ez0uj10uXbYfOQTIubzAy/hxJJn1kNbHr6Q=="
            ;

        // This method just verifies the digital signing with the Public Key
        Test_VerifySign(content, signature);

        Console.WriteLine("\n\n+++ Program completed +++\n");
        Console.ReadLine();
    }

    private static void Test_VerifySign(string content, string signature)
    {
        Console.WriteLine("\n\n=== ECDSA - Just Verify Signature ===\n");
        //Console.WriteLine($"Signature:\t\t{signature}");
        Console.Write($"Signature:");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"\t\t{signature}\n");
        Console.ResetColor();

        byte[] contentRaw = File.ReadAllBytes(content);

        bool valid;
        string publicKeyFile = @"pub8.der";
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

        ISign signButVerify = new SignButVerify();

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
        string pubKeyFile = @"pub8.der";
        byte[] publicKeyRaw = File.ReadAllBytes(pubKeyFile);
        string publicKeyStr = Convert.ToBase64String(publicKeyRaw);
        Console.WriteLine($"Public Key:\t\t{publicKeyStr}");

        // verify signature
        // Public Key loading using ECDSA, not ECDSACng
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

        ECDsa eCDsa = LoadPublicKey(pubKey);
        result = eCDsa.VerifyData(msg, signature, HashAlgorithmName.SHA256);
        return result;
    }

    private static ECDsa LoadPublicKey(byte[] pubKey)
    {
        ECDsa eCDsa = ECDsa.Create();
        eCDsa.ImportSubjectPublicKeyInfo(pubKey, out _);
        return eCDsa;
    }




    private static ECDsa LoadPrivateKey(string content, string pwd)
    {
        var priv = File.ReadAllBytes(content);
        byte[] pwdBytes = Encoding.UTF8.GetBytes(pwd);

        ECDsa eCDsa = ECDsa.Create();
        eCDsa.ImportPkcs8PrivateKey(priv, out _);
        //eCDsa.ImportEncryptedPkcs8PrivateKey(pwdBytes, priv, out _);

        Console.WriteLine($"OK: Private Key loaded...: {eCDsa.SignatureAlgorithm} ready.");
        return eCDsa;
    }

    private static byte[] Hash(byte[] msg)
    {
        ECDsa eCDsa = ECDsa.Create();
        byte[] hash = eCDsa.SignHash(msg);
        Console.WriteLine($"Hash Value: {Convert.ToBase64String(hash)}");
        return hash;
    }

    private static byte[] Sign(byte[] msg, ECDsa eCDsa)
    {
        byte[] result = eCDsa.SignData(msg, HashAlgorithmName.SHA256);
        return result;
    }

    private static bool VerifyHash(byte[] msg, byte[] signature, byte[] pubKey)
    {
        bool result;

        ECDsa eCDsa = LoadPublicKey(pubKey);
        result = eCDsa.VerifyHash(msg, signature);
        return result;
    }

}