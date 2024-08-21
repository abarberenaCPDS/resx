using Azure.Storage.Sas;
using System;

namespace PreSignedUrlGenerator
{
    public interface IStorageUrlGenerator
    {
        string GenerateBlobSasUrl(string containerName, string blobName, BlobSasPermissions permissions, TimeSpan expiryDuration);
    }
}