using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using clipVault.Models.Images.UploadThumbnail;
using clipVault.Models.Videos.UploadVideo;
using MediatR;
using Microsoft.Extensions.Logging;
using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace clipVault.Handlers.Videos
{
    public class UploadVideoHandler : IRequestHandler<UploadVideoRequest, UploadVideoResponse>
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IMediator _mediator;

        public UploadVideoHandler(BlobServiceClient blobServiceClient, IMediator mediator)
        {
            _blobServiceClient = blobServiceClient;
            _mediator = mediator;
        }

        public async Task<UploadVideoResponse> Handle(UploadVideoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var videoContainerClient = _blobServiceClient.GetBlobContainerClient("videostore");
                var videoBlobClient = videoContainerClient.GetBlobClient(request.File.FileName);

                var id = Guid.NewGuid().ToString();
                var tags = request.Tags ??= "";

                var metadata = new Dictionary<string, string>
                {
                    { "id", id},
                    { "name", request.Name },
                    { "tags", tags}
                };

                using (var stream = request.File.OpenReadStream())
                {
                    await videoBlobClient.UploadAsync(stream, true, cancellationToken);
                    await videoBlobClient.SetMetadataAsync(metadata, cancellationToken: cancellationToken);
                }

                var thumbnail = GenerateThumbnail(request.File);
                var thumbnailRequest = new UploadThumbnailRequest
                {
                    File = request.File,
                    Thumbnail = thumbnail,
                    Metadata = metadata
                };

                await _mediator.Send(thumbnailRequest, cancellationToken);

                return new UploadVideoResponse { Id = id, Message = "File uploaded successfully." };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Issue uploading clip: {{request.Name}}", ex);
            }
        }

        private byte[] GenerateThumbnail(IFormFile file)
        {
            var ffmpeg = new FFMpegConverter();
            using (var inputStream = file.OpenReadStream())
            using (var outputStream = new MemoryStream())
            {
                var tempFilePath = Path.GetTempFileName();
                using (var tempFileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    inputStream.CopyTo(tempFileStream);
                }
                ffmpeg.GetVideoThumbnail(tempFilePath, outputStream, 5);
                File.Delete(tempFilePath);
                return outputStream.ToArray();
            }
        }
    }
}
