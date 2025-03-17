using clipVault.Models.Video.DeleteVideo;
using clipVault.Repositories.Video;

namespace clipVault.Scenarios.Video
{
    public class DeleteVideoScenario : IDeleteVideoScenario
    {
        private readonly IVideoRepository _videoRepository;

        public DeleteVideoScenario(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        public async Task<DeleteVideoResponse> ExecuteAsync(DeleteVideoRequest request, CancellationToken cancellationToken)
        {
            var videoDeleted = await _videoRepository.DeleteVideoAsync(request.Id, cancellationToken);
            var thumbnailDeleted = await _videoRepository.DeleteThumbnailAsync(request.Id, cancellationToken);

            if (!videoDeleted || !thumbnailDeleted)
            {
                return new DeleteVideoResponse { Message = "Failed to delete both video and thumbnail", Success = false };
            }

            return new DeleteVideoResponse { Message = "Video and thumbnail deleted successfully", Success = true };
        }
    }
}
