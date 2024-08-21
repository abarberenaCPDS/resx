namespace PreSignedUrlGenerator
{
    public static class StorageUrlGeneratorFactory
    {
        public static IStorageUrlGenerator Create(string accountName, string accountKey)
        {
            // In the future, you could add logic to create different types of generators
            // For now, we only return the standard StorageUrlGenerator
            return new StorageUrlGenerator(accountName, accountKey);
        }
    }
}