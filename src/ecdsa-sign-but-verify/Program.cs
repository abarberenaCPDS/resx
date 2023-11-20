using System;
using System.Security.Cryptography;
using System.Text;

namespace EcdsaSignAndVerify;

class Program
{
    static string content;

    static void Main(string[] args)
    {
        // TODO: Test using the default file ...
        if (!args.Any())
            args = new[] { @"sample.json" };

        content = args[0];
        //data = Encoding.UTF8.GetBytes("abe");
        Test_SignAndVerify();
    }

    private static void Test_SignAndVerify()
    {
        Console.WriteLine("\n=== ECDSA Sign but Verify ===\n");
        // string content = @"sample.json";
        byte[] data = File.ReadAllBytes(content);

        string privateKeyFile = @"prv.der";
        string pwd =
            "P@ssw0rd"
        ;

        ECDsa eCDsa = LoadPrivateKey(privateKeyFile, pwd);
        byte[] signed = Sign(data, eCDsa);
        Console.WriteLine($"Signature: {Convert.ToBase64String(signed)}");

        bool valid;
        string pubKeyFile = @"pub.der";
        byte[] pub = File.ReadAllBytes(pubKeyFile);
        Console.WriteLine($"Public Key: {Convert.ToBase64String(pub)}");

        // valid payload
        valid = Verify(data, signed, pub);

        Console.Write($"\nIs Valid? --> ");
        Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
        Console.Write($"{valid.ToString().ToUpper()}"); Console.ResetColor();

        // tampered payload
        content = @"sample-tampered.json";
        data = File.ReadAllBytes(content);

        valid = Verify(data, signed, pub);

        Console.Write($"\nIs Valid? --> ");
        Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
        Console.Write($"{valid.ToString().ToUpper()}"); Console.ResetColor();

        Console.WriteLine("\n\n=== Program completed ===\n");
    }

    private static ECDsa LoadPrivateKey(string content, string pwd)
    {
        var priv = File.ReadAllBytes(content);
        byte[] pwdBytes = Encoding.UTF8.GetBytes(pwd);

        ECDsa eCDsa = ECDsa.Create();

        eCDsa.ImportEncryptedPkcs8PrivateKey(pwdBytes, priv, out _);

        // TODO: refactor this later...
        // PbeParameters pbeParameters =
        //     new PbeParameters(PbeEncryptionAlgorithm.Aes128Cbc, HashAlgorithmName.SHA256, 2048);
        // var exportedKey = eCDsa.ExportEncryptedPkcs8PrivateKey(pwd, pbeParameters);

        Console.WriteLine($"OK: Private Key loaded...: {eCDsa.SignatureAlgorithm} ready.");
        return eCDsa;
    }

    private static void Hash(byte[] msg)
    {
        ECDsa eCDsa = ECDsa.Create();
        eCDsa.SignHash(msg);
    }

    private static byte[] Sign(byte[] msg, ECDsa eCDsa)
    {
        byte[] result = eCDsa.SignData(msg, HashAlgorithmName.SHA256);
        return result;
    }

    private static bool Verify(byte[] msg, byte[] signature, byte[] pubKey)
    {
        bool result;

        ECDsa eCDsa = ECDsa.Create();
        eCDsa.ImportSubjectPublicKeyInfo(pubKey, out _);
        result = eCDsa.VerifyData(msg, signature, HashAlgorithmName.SHA256);
        return result;
    }

    private static void abes()
    {
        var ecdsa = ECDsa.Create();
        var priv = File.ReadAllBytes(@"prv.der");
        var pub = File.ReadAllBytes(@"pub.der");

        var json = File.ReadAllBytes(@"sample.json");

        // ecdsa.ImportFromPem(priv);
        string pwd =
            "P@ssw0rd"
        //string.Empty
        ;
        // byte[] pwdBytes = Encoding.UTF8.GetBytes(pwd);
        byte[] pwdBytes = Encoding.UTF8.GetBytes(pwd);

        // ecdsa.ImportPkcs8PrivateKey(priv, out _);

        try
        {
            ecdsa.ImportEncryptedPkcs8PrivateKey(pwdBytes, priv, out _);
            Console.WriteLine($"OK: {ecdsa.SignatureAlgorithm}");

            byte[] msgHash = ecdsa.SignHash(json);

            // byte[] signed = ecdsa.SignData(json, HashAlgorithmName.SHA256);

            byte[] signature = ecdsa.SignData(msgHash, HashAlgorithmName.SHA256);

            bool result = ecdsa.VerifyData(msgHash, signature, HashAlgorithmName.SHA256);
            ecdsa.Clear();

            var res = $"Is Valid: {result}";
            Console.WriteLine(res);
        }
        catch (CryptographicException ex)
        {
            //The EncryptedPrivateKeyInfo structure was decoded but was not successfully interpreted, the password may be incorrect.
            Console.WriteLine(ex.Message);
        }

    }
}