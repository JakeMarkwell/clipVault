using clipVault.Models.Images.GetThumbnail;
using clipVault.Models.Images.GetThumbnails;

namespace clipVault.Repositories.Images
{
    public interface IThumbnailService
    {
        Task<GetThumbnailResponse> GetThumbnailAsync(string id, CancellationToken cancellationToken);
        Task<GetThumbnailsResponse> GetThumbnailsAsync(int limit, CancellationToken cancellationToken); // <-- Add this

        Task<bool> DeleteThumbnailAsync(string id, CancellationToken cancellationToken);
        Task<byte[]> GenerateThumbnailAsync(IFormFile file, CancellationToken cancellationToken);
    }
}
