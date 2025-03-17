using clipVault.Models.Video.DeleteVideo;

namespace clipVault.Scenarios.Video
{
    public interface IDeleteVideoScenario
    {
        Task<DeleteVideoResponse> ExecuteAsync(DeleteVideoRequest request, CancellationToken cancellationToken);
    }
}
