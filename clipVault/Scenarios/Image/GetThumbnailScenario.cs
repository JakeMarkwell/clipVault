using clipVault.Models.Images.GetThumbnail;
using clipVault.Repositories.Images;

namespace clipVault.Scenarios.Images
{
    public class GetThumbnailScenario : IGetThumbnailScenario
    {
        private readonly IGetThumbnailRepository _repository;

        public GetThumbnailScenario(IGetThumbnailRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetThumbnailResponse> HandleAsync(GetThumbnailRequest request, CancellationToken cancellationToken)
        {
            var imageData = await _repository.GetThumbnailAsync(request.id, cancellationToken);

            return new GetThumbnailResponse
            {
                imageData = imageData,
                fileType = "image/png" //Always want the image to be png
            };
        }
    }
}
