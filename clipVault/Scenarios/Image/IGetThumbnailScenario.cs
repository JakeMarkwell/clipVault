using clipVault.Models.Images.GetThumbnail;

namespace clipVault.Scenarios.Images
{
    public interface IGetThumbnailScenario
    {
        Task<GetThumbnailResponse> HandleAsync(GetThumbnailRequest request, CancellationToken cancellationToken);
    }
}
