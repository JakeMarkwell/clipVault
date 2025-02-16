using Azure.Storage.Blobs;
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

        public async Task<byte[]> GenerateThumbnailAsync(IFormFile file, CancellationToken cancellationToken)
        {
            return await _thumbnailGenerator.GenerateThumbnailAsync(file, cancellationToken);
        }
    }
}
