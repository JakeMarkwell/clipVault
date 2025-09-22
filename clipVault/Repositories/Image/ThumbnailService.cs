using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using clipVault.Models.Images.GetThumbnail;
using clipVault.Models.Images.GetThumbnails;
using NReco.VideoConverter;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace clipVault.Repositories.Images
{
    public class ThumbnailService : IThumbnailService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public ThumbnailService(BlobServiceClient blobServiceClient)
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
                    var friendTags = properties.Value.Metadata["friendTags"];

                    // Parse categoryIds from metadata
                    var categoryIdsString = properties.Value.Metadata.TryGetValue("categoryIds", out var ids) ? ids : "";
                    var categoryIds = categoryIdsString
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => int.TryParse(s, out var i) ? i : (int?)null)
                        .Where(i => i.HasValue)
                        .Select(i => i.Value)
                        .ToList();

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
                            categoryIds = categoryIds
                        };
                    }
                }
            }
            return null;
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
                    string friendTags = string.Empty;
                    if (properties.Value.Metadata.TryGetValue("friendTags", out var friendTagsString))
                    {
                        friendTags = friendTagsString;
                    }

                    string categoryIdsString = string.Empty;
                    if (properties.Value.Metadata.TryGetValue("categoryIds", out var categoryIdsMeta))
                    {
                        categoryIdsString = categoryIdsMeta;
                    }

                    var downloadInfo = await blobClient.DownloadAsync(cancellationToken);
                    using (var memoryStream = new MemoryStream())
                    {
                        await downloadInfo.Value.Content.CopyToAsync(memoryStream, cancellationToken);
                        var imageData = memoryStream.ToArray();

                        var categoryIds = categoryIdsString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => int.TryParse(s, out var i) ? i : (int?)null)
                            .Where(i => i.HasValue)
                            .Select(i => i.Value)
                            .ToList();

                        thumbnails.Add(new ThumbnailItem
                        {
                            Id = id,
                            Title = title,
                            ImageData = imageData,
                            FileType = properties.Value.ContentType ?? "image/png",
                            FriendTags = friendTags,
                            CategoryIds = categoryIds
                        });
                    }

                    if (thumbnails.Count >= limit)
                        break;
                }
            }

            response.Thumbnails = thumbnails.OrderBy(t => t.Title).ToList();
            return response;
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

        public async Task<bool> DeleteThumbnailAsync(string id, CancellationToken cancellationToken)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("imagestore");
            var blobs = containerClient.GetBlobsAsync(BlobTraits.Metadata, cancellationToken: cancellationToken);
            await foreach (var blob in blobs)
            {
                if (blob.Metadata.ContainsKey("id") && blob.Metadata["id"] == id)
                {
                    var blobClient = containerClient.GetBlobClient(blob.Name);
                    var response = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
                    return response.Value;
                }
            }

            return false;
        }

        public async Task<byte[]> GenerateThumbnailAsync(IFormFile file, CancellationToken cancellationToken)
        {
            var ffmpeg = new FFMpegConverter();
            using (var inputStream = file.OpenReadStream())
            using (var outputStream = new MemoryStream())
            {
                var tempFilePath = Path.GetTempFileName();
                using (var tempFileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    await inputStream.CopyToAsync(tempFileStream, cancellationToken);
                }
                ffmpeg.GetVideoThumbnail(tempFilePath, outputStream, 5);
                File.Delete(tempFilePath);
                return outputStream.ToArray();
            }
        }
    }
}
