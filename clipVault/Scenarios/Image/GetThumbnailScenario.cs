using clipVault.Models.Images.GetThumbnail;
using clipVault.Repositories.Images;

namespace clipVault.Scenarios.Images
{
    public class GetThumbnailScenario : IGetThumbnailScenario
    {
        private readonly IGetThumbnailService _repository;

        public GetThumbnailScenario(IGetThumbnailService repository)
        {
            _repository = repository;
        }

        public async Task<GetThumbnailResponse> GetThumbnail(GetThumbnailRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetThumbnailAsync(request.id, cancellationToken);

        }
    }
}
