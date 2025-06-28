using clipVault.Models.Images.UploadThumbnail;
using clipVault.Models.Videos.UploadVideo;
using clipVault.Repositories.Images;
using clipVault.Repositories.Video;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace clipVault.Handlers.Videos
{
    public class UploadVideoHandler : IRequestHandler<UploadVideoRequest, UploadVideoResponse>
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IThumbnailService _thumbnailService;
        private readonly IMediator _mediator;

        public UploadVideoHandler(IVideoRepository videoRepository, IMediator mediator, IThumbnailService thumbnailService)
        {
            _videoRepository = videoRepository;
            _thumbnailService = thumbnailService;
            _mediator = mediator;
        }

        public async Task<UploadVideoResponse> Handle(UploadVideoRequest request, CancellationToken cancellationToken)
        {
            var metadata = new Dictionary<string, string>
            {
                { "id", Guid.NewGuid().ToString() },
                { "title", request.Title },
                { "friendTags", request.FriendTags.ToLower()},
                { "categoryTags", request.CategoryTags.ToLower()}
            };

            await _videoRepository.UploadVideoAsync(request.File, metadata, cancellationToken);

            var thumbnail = await _thumbnailService.GenerateThumbnailAsync(request.File, cancellationToken);
            var thumbnailRequest = new UploadThumbnailRequest
            {
                File = request.File,
                Thumbnail = thumbnail,
                Metadata = metadata,
                Title = request.Title,
                FriendTags = request.FriendTags,
                CategoryTags = request.CategoryTags
            };

            await _mediator.Send(thumbnailRequest, cancellationToken);

            return new UploadVideoResponse { Id = metadata["id"], Message = "File uploaded successfully." };
        }
    }
}