using clipVault.Models.Images.UploadThumbnail;
using clipVault.Models.Videos.UploadVideo;
using clipVault.Repositories.Video;
using MediatR;

namespace clipVault.Scenarios.Video
{
    public class UploadVideoScenario : IUploadVideoScenario
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IMediator _mediator;

        public UploadVideoScenario(IVideoRepository videoRepository, IMediator mediator)
        {
            _videoRepository = videoRepository;
            _mediator = mediator;
        }

        public async Task<UploadVideoResponse> ExecuteAsync(UploadVideoRequest request, CancellationToken cancellationToken)
        {
            var metadata = new Dictionary<string, string>
            {
                { "id", Guid.NewGuid().ToString() }
            };

            await _videoRepository.UploadVideoAsync(request.File, metadata, cancellationToken);

            var thumbnail = await _videoRepository.GenerateThumbnailAsync(request.File, cancellationToken);
            var thumbnailRequest = new UploadThumbnailRequest
            {
                File = request.File,
                Thumbnail = thumbnail,
                Metadata = metadata
            };

            await _mediator.Send(thumbnailRequest, cancellationToken);

            return new UploadVideoResponse { Id = metadata["id"], Message = "File uploaded successfully." };
        }
    }
}
