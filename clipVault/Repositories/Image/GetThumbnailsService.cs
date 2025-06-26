using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using clipVault.Models.Images.GetThumbnails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace clipVault.Repositories.Images
{
    public class GetThumbnailsService : IGetThumbnailsService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public GetThumbnailsService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<GetThumbnailsResponse> GetThumbnailsAsync(int limit, CancellationToken cancellationToken)
        {
            var response = new GetThumbnailsResponse();
            var containerClient = _blobServiceClient.GetBlobContainerClient("imagestore");
            
            var thumbnails = new List<ThumbnailItem>();
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(cancellationToken: cancellationToken))
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);
                var properties = await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken);

                if (properties.Value.Metadata.TryGetValue("id", out var id) && 
                    properties.Value.Metadata.TryGetValue("title", out var title))
                {
                    // Extract friend tags and category tags if they exist
                    string friendTags = string.Empty;
                    if (properties.Value.Metadata.TryGetValue("friendTags", out var friendTagsString))
                    {
                        friendTags = friendTagsString;
                    }

                    string categoryTags = string.Empty;
                    if (properties.Value.Metadata.TryGetValue("categoryTags", out var categoryTagsString))
                    {
                        categoryTags = categoryTagsString;
                    }

                    var downloadInfo = await blobClient.DownloadAsync(cancellationToken);
                    using (var memoryStream = new MemoryStream())
                    {
                        await downloadInfo.Value.Content.CopyToAsync(memoryStream, cancellationToken);
                        var imageData = memoryStream.ToArray();
                        
                        thumbnails.Add(new ThumbnailItem
                        {
                            Id = id,
                            Title = title,
                            ImageData = imageData,
                            FileType = properties.Value.ContentType ?? "image/png",
                            FriendTags = friendTags,
                            CategoryTags = categoryTags
                        });
                    }

                    if (thumbnails.Count >= limit)
                        break;
                }
            }

            // Order alphabetically by title
            response.Thumbnails = thumbnails.OrderBy(t => t.Title).ToList();
            return response;
        }
    }
}
