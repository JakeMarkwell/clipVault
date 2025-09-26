using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;

namespace clipVault.Handlers.Video
{
    public class GetUploadSasHandler
    {
        private readonly IConfiguration _configuration;

        public GetUploadSasHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateSasUrl(string fileName)
        {
            var connectionString = _configuration.GetConnectionString("AzureBlobStorage");
            var containerName = "videostore"; 

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = fileName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.All);

            var accountName = blobServiceClient.AccountName;
            var accountKey = connectionString.Split("AccountKey=")[1].Split(';')[0];
            var sharedKeyCredential = new Azure.Storage.StorageSharedKeyCredential(accountName, accountKey);

            var sasToken = sasBuilder.ToSasQueryParameters(sharedKeyCredential).ToString();
            var sasUrl = $"{blobClient.Uri}?{sasToken}";

            return sasUrl;
        }
    }
}
