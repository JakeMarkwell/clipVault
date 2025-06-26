using clipVault.Models.Images.GetThumbnails;
using System.Threading;
using System.Threading.Tasks;

namespace clipVault.Repositories.Images
{
    public interface IGetThumbnailsService
    {
        Task<GetThumbnailsResponse> GetThumbnailsAsync(int limit, CancellationToken cancellationToken);
    }
}
