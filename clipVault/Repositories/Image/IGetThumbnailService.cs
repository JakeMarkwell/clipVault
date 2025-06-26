using clipVault.Models.Images.GetThumbnail;

namespace clipVault.Repositories.Images
{
    public interface IGetThumbnailService
    {
        Task<GetThumbnailResponse> GetThumbnailAsync(string id, CancellationToken cancellationToken);
    }
}
