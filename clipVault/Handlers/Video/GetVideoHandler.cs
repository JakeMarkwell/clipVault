using clipVault.Models.Video.GetVideo;
using clipVault.Repositories.Video;
using MediatR;

namespace clipVault.Handlers.Video
{
    public class GetVideoHandler : IRequestHandler<GetVideoRequest, GetVideoResponse>
    {
        private readonly IVideoRepository _videoRepository;

        public GetVideoHandler(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        public async Task<GetVideoResponse> Handle(GetVideoRequest request, CancellationToken cancellationToken)
        {
            var video = await _videoRepository.GetVideoAsync(request.Id, cancellationToken);
            if (video is null)
                return null;

            // Generate SAS URL for secure access
            var videoSasUrl = await _videoRepository.GetVideoSasUrlAsync(request.Id, cancellationToken);

            return new GetVideoResponse
            {
                Id = video?.Id ?? string.Empty,
                Title = video?.Title ?? string.Empty,
                CategoryTags = video?.CategoryTags ?? string.Empty,
                FriendTags = video?.FriendTags ?? string.Empty,
                VideoUrl = videoSasUrl, // Use SAS URL
                ContentType = video?.ContentType ?? string.Empty
            };
        }
    }
}
