using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using clipVault.Models.Video.GetVideo;
using clipVault.Services.Images;

namespace clipVault.Repositories.Video
{
    public class VideoRepository : IVideoRepository
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IThumbnailGenerator _thumbnailGenerator;
        private readonly string _containerName = "videos";


        public VideoRepository(BlobServiceClient blobServiceClient, IThumbnailGenerator thumbnailGenerator)
        {
            _blobServiceClient = blobServiceClient;
            _thumbnailGenerator = thumbnailGenerator;
        }

        public async Task UploadVideoAsync(IFormFile file, Dictionary<string, string> metadata, CancellationToken cancellationToken)
        {
            var videoContainerClient = _blobServiceClient.GetBlobContainerClient("videostore");
            var videoBlobClient = videoContainerClient.GetBlobClient(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                await videoBlobClient.UploadAsync(stream, true, cancellationToken);
                await videoBlobClient.SetMetadataAsync(metadata, cancellationToken: cancellationToken);
            }
        }

        public async Task<bool> DeleteVideoAsync(string id, CancellationToken cancellationToken)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("videostore");
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
            return await _thumbnailGenerator.GenerateThumbnailAsync(file, cancellationToken);
        }

        public async Task<VideoDto?> GetVideoAsync(string videoGuid, CancellationToken cancellationToken)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("videostore");
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(BlobTraits.Metadata, cancellationToken: cancellationToken))
            {
                if (blobItem.Metadata.TryGetValue("id", out var blobId) && blobId == videoGuid)
                {
                    var blobClient = containerClient.GetBlobClient(blobItem.Name);
                    var properties = await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken);

                    var title = properties.Value.Metadata.TryGetValue("title", out var t) ? t : string.Empty;
                    var categoryTags = properties.Value.Metadata.TryGetValue("categoryTags", out var c) ? c : string.Empty;
                    var friendTags = properties.Value.Metadata.TryGetValue("friendTags", out var f) ? f : string.Empty;
                    var contentType = properties.Value.ContentType ?? "video/mp4";
                    var videoUrl = blobClient.Uri.ToString();

                    return new VideoDto
                    {
                        Id = videoGuid,
                        Title = title,
                        CategoryTags = categoryTags,
                        FriendTags = friendTags,
                        VideoUrl = videoUrl,
                        ContentType = contentType
                    };
                }
            }
            return null;
        }

        public async Task<string> GetVideoSasUrlAsync(string videoId, CancellationToken cancellationToken)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("videostore");
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(BlobTraits.Metadata, cancellationToken: cancellationToken))
            {
                if (blobItem.Metadata.TryGetValue("id", out var blobId) && blobId == videoId)
                {
                    var blobClient = containerClient.GetBlobClient(blobItem.Name);

                    var sasBuilder = new BlobSasBuilder
                    {
                        BlobContainerName = containerClient.Name,
                        BlobName = blobClient.Name,
                        Resource = "b",
                        ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10)
                    };
                    sasBuilder.SetPermissions(BlobSasPermissions.Read);

                    var sasUri = blobClient.GenerateSasUri(sasBuilder);
                    return sasUri.ToString();
                }
            }
            return string.Empty;
        }

    }

}


