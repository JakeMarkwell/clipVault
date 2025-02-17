using clipVault.Models.Videos.UploadVideo;

namespace clipVault.Scenarios.Video
{
    public interface IUploadVideoScenario
    {
        Task<UploadVideoResponse> ExecuteAsync(UploadVideoRequest request, CancellationToken cancellationToken);
    }
}
