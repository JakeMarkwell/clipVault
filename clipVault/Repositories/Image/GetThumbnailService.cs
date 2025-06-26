using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using clipVault.Models.Images.GetThumbnail;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace clipVault.Repositories.Images
{
    public class GetThumbnailService : IGetThumbnailService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public GetThumbnailService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<GetThumbnailResponse> GetThumbnailAsync(string id, CancellationToken cancellationToken)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("imagestore");
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(cancellationToken: cancellationToken))
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);
                var properties = await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken);

                if (properties.Value.Metadata.TryGetValue("id", out var blobId) && blobId == id)
                {
                    var videoTitle = properties.Value.Metadata["title"];

                    // Extract friend tags and category tags if they exist
                    string[] friendTags = Array.Empty<string>();
                    if (properties.Value.Metadata.TryGetValue("friendTags", out var friendTagsString))
                    {
                        friendTags = friendTagsString.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    }

                    string[] categoryTags = Array.Empty<string>();
                    if (properties.Value.Metadata.TryGetValue("categoryTags", out var categoryTagsString))
                    {
                        categoryTags = categoryTagsString.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    }

                    var downloadInfo = await blobClient.DownloadAsync(cancellationToken);
                    using (var memoryStream = new MemoryStream())
                    {
                        await downloadInfo.Value.Content.CopyToAsync(memoryStream, cancellationToken);
                        var imageData = memoryStream.ToArray();
                        return new GetThumbnailResponse
                        {
                            imageData = imageData,
                            fileType = "image/png",
                            title = videoTitle,
                            friendTags = friendTags,
                            categoryTags = categoryTags
                        };
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
