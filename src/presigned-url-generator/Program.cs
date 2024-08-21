using System;
using Azure.Storage.Sas;

namespace PreSignedUrlGenerator;
class Program
{
    static void Main(string[] args)
    {
        string storageAccountName = "devstoreaccount1";
        string storageAccountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";
        string containerName = "my-container";
        string blobName = "my-file.txt";

        // Use the factory to create the generator
        IStorageUrlGenerator urlGenerator = StorageUrlGeneratorFactory.Create(storageAccountName, storageAccountKey);

        // Generate the SAS URL
        string sasUrl = urlGenerator.GenerateBlobSasUrl(containerName, blobName, BlobSasPermissions.Read | BlobSasPermissions.Write, TimeSpan.FromHours(1));

        Console.WriteLine(string.Concat("Generated SAS URL: " , sasUrl));
    }
}
