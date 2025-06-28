using clipVault.Models.Video.GetVideo;
using Microsoft.AspNetCore.Http;

namespace clipVault.Repositories.Video
{
    public interface IVideoRepository
    {
        Task<bool> DeleteVideoAsync(string id, CancellationToken cancellationToken);
        Task<bool> DeleteThumbnailAsync(string id, CancellationToken cancellationToken);
        Task UploadVideoAsync(IFormFile video, Dictionary<string, string> metadata, CancellationToken cancellationToken);
        Task<byte[]> GenerateThumbnailAsync(IFormFile video, CancellationToken cancellationToken);
        Task<VideoDto> GetVideoAsync(string videoGuid, CancellationToken cancellationToken);
        Task<string> GetVideoSasUrlAsync(string videoId, CancellationToken cancellationToken);


    }
}