using clipVault.Models.Images.GetThumbnail;
using clipVault.Repositories.Images;
using clipVault.Scenarios.Images;
using MediatR;

namespace clipVault.Handlers.Images
{
    public class GetThumbnailHandler : IRequestHandler<GetThumbnailRequest, GetThumbnailResponse>
    {
        private readonly IGetThumbnailScenario _scenario;

        public GetThumbnailHandler(IGetThumbnailScenario scenario)
        {
            _scenario = scenario;
        }

        public async Task<GetThumbnailResponse> Handle(GetThumbnailRequest request, CancellationToken cancellationToken)
        {
            return await _scenario.GetThumbnail(request, cancellationToken);
        }
    }
}
