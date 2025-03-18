using clipVault.Models.Images.GetThumbnail;

namespace clipVault.Repositories.Images
{
    public interface IGetThumbnailRepository
    {
        Task<GetThumbnailResponse> GetThumbnailAsync(string id, CancellationToken cancellationToken);
    }
}
