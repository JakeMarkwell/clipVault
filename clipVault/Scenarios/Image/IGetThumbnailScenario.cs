using clipVault.Models.Images.GetThumbnail;

namespace clipVault.Scenarios.Images
{
    public interface IGetThumbnailScenario
    {
        Task<GetThumbnailResponse> GetThumbnail(GetThumbnailRequest request, CancellationToken cancellationToken);
    }
}
