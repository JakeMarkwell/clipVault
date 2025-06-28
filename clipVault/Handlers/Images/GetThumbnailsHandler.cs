using clipVault.Models.Images.GetThumbnails;
using clipVault.Repositories.Images;
using MediatR;

namespace clipVault.Handlers.Images
{
    public class GetThumbnailsHandler : IRequestHandler<GetThumbnailsRequest, GetThumbnailsResponse>
    {
        private readonly IThumbnailService _thumbnailService;

        public GetThumbnailsHandler(IThumbnailService thumbnailService)
        {
            _thumbnailService = thumbnailService;
        }

        public async Task<GetThumbnailsResponse> Handle(GetThumbnailsRequest request, CancellationToken cancellationToken)
        {
            return await _thumbnailService.GetThumbnailsAsync(request.Limit, cancellationToken);
        }
    }
}
