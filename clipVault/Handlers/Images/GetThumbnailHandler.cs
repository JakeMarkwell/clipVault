using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using clipVault.Models.Images.GetThumbnail;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace clipVault.Handlers.Images
{
    public class GetThumbnailHandler : IRequestHandler<GetThumbnailRequest, GetThumbnailResponse>
    {
        private readonly BlobServiceClient _blobServiceClient;

        public GetThumbnailHandler(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<GetThumbnailResponse> Handle(GetThumbnailRequest request, CancellationToken cancellationToken)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("imagestore");
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(cancellationToken: cancellationToken))
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);
                var properties = await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken);

                if (properties.Value.Metadata.TryGetValue("id", out var id) && id == request.id)
                {
                    var response = new GetThumbnailResponse();
                    var downloadInfo = await blobClient.DownloadAsync(cancellationToken);

                    using (var memoryStream = new MemoryStream())
                    {
                        await downloadInfo.Value.Content.CopyToAsync(memoryStream, cancellationToken);
                        response.imageData = memoryStream.ToArray();
                        response.fileType = downloadInfo.Value.Details.ContentType;
                    }

                    return response;
                }
            }

            return null;
        }
    }
}