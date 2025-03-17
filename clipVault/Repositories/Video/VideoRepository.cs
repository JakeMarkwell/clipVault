using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using clipVault.Services.Images;

namespace clipVault.Repositories.Video
{
    public class VideoRepository : IVideoRepository
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IThumbnailGenerator _thumbnailGenerator;

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
    }
}
