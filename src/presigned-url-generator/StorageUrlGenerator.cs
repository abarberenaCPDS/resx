using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using System;

namespace PreSignedUrlGenerator
{
    public class StorageUrlGenerator : IStorageUrlGenerator
    {
        private readonly string storageAccountName;
        private readonly string storageAccountKey;

        public StorageUrlGenerator(string accountName, string accountKey)
        {
            storageAccountName = accountName;
            storageAccountKey = accountKey;
        }

        public string GenerateBlobSasUrl(string containerName, string blobName, BlobSasPermissions permissions, TimeSpan expiryDuration)
        {
            string uri = 
                // $"https://{storageAccountName}.blob.core.windows.net"
                $"http://127.0.0.1:10000/devstoreaccount1"
            ;
            // Create a BlobServiceClient
            BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri(uri), new StorageSharedKeyCredential(storageAccountName, storageAccountKey));

            // Get container and blob clients
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Create the SAS token
            BlobSasBuilder sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b",  // "b" for blob
                ExpiresOn = DateTimeOffset.UtcNow.Add(expiryDuration)
            };

            // Set permissions
            sasBuilder.SetPermissions(permissions);

            // Generate and return the SAS token URL
            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
            return sasUri.ToString();
        }
    }
}
