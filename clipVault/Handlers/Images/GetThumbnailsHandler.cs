using clipVault.Models.Images.GetThumbnails;
using clipVault.Repositories.Images;
using MediatR;

namespace clipVault.Handlers.Images
{
    public class GetThumbnailsHandler : IRequestHandler<GetThumbnailsRequest, GetThumbnailsResponse>
    {
        private readonly IGetThumbnailsService _thumbnailsService;

        public GetThumbnailsHandler(IGetThumbnailsService thumbnailsService)
        {
            _thumbnailsService = thumbnailsService;
        }

        public async Task<GetThumbnailsResponse> Handle(GetThumbnailsRequest request, CancellationToken cancellationToken)
        {
            return await _thumbnailsService.GetThumbnailsAsync(request.Limit, cancellationToken);
        }
    }
}
