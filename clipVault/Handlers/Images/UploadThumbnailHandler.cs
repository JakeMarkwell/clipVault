using Azure.Storage.Blobs;
using clipVault.Models.Images.UploadThumbnail;
using MediatR;
using System.IO;
using System.Linq;
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

            // Add friend tags and category tags to metadata if they exist
            if (request.FriendTags?.Any() == true)
            {
                request.Metadata["friendTags"] = string.Join(",", request.FriendTags);
            }

            if (request.CategoryIds?.Any() == true)
            {
                request.Metadata["categoryIds"] = string.Join(",", request.CategoryIds);
            }

            using (var thumbnailStream = new MemoryStream(request.Thumbnail))
            {
                await thumbnailBlobClient.UploadAsync(thumbnailStream, true, cancellationToken);
                await thumbnailBlobClient.SetMetadataAsync(request.Metadata, cancellationToken: cancellationToken);
            }

            return new UploadThumbnailResponse { Message = "Thumbnail uploaded successfully." };
        }
    }
}