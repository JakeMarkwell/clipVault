using Azure.Storage.Blobs;
using clipVault.Models.Images;
using clipVault.Models.Images.UploadThumbnail;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace clipVault.Handlers.Images
{
    public class UploadThumbnailHandler : IRequestHandler<UploadThumbnailRequest, UploadThumbnailResponse>
    {
        private readonly BlobServiceClient _blobServiceClient;

        public UploadThumbnailHandler(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<UploadThumbnailResponse> Handle(UploadThumbnailRequest request, CancellationToken cancellationToken)
        {
            var thumbnailContainerClient = _blobServiceClient.GetBlobContainerClient("imagestore");
            var thumbnailBlobClient = thumbnailContainerClient.GetBlobClient(Path.GetFileNameWithoutExtension(request.File.FileName) + ".png");

            using (var thumbnailStream = new MemoryStream(request.Thumbnail))
            {
                await thumbnailBlobClient.UploadAsync(thumbnailStream, true, cancellationToken);
                await thumbnailBlobClient.SetMetadataAsync(request.Metadata, cancellationToken: cancellationToken);
            }

            return new UploadThumbnailResponse { Message = "Thumbnail uploaded successfully." };
        }
    }
}
