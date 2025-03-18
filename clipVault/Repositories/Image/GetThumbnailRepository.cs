using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace clipVault.Repositories.Images
{
    public class GetThumbnailRepository : IGetThumbnailRepository
    {
        private readonly BlobServiceClient _blobServiceClient;

        public GetThumbnailRepository(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<byte[]> GetThumbnailAsync(string id, CancellationToken cancellationToken)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("imagestore");
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(cancellationToken: cancellationToken))
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);
                var properties = await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken);

                if (properties.Value.Metadata.TryGetValue("id", out var blobId) && blobId == id)
                {
                    var downloadInfo = await blobClient.DownloadAsync(cancellationToken);
                    using (var memoryStream = new MemoryStream())
                    {
                        await downloadInfo.Value.Content.CopyToAsync(memoryStream, cancellationToken);
                        var returner =  memoryStream.ToArray();
                        return returner;
                    }
                }
            }

            return null;
        }

        public async Task<string> GetThumbnailContentTypeAsync(string id, CancellationToken cancellationToken)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("imagestore");
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(cancellationToken: cancellationToken))
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);
                var properties = await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken);

                if (properties.Value.Metadata.TryGetValue("id", out var blobId) && blobId == id)
                {
                    return properties.Value.ContentType;
                }
            }

            return null;
        }
    }
}
