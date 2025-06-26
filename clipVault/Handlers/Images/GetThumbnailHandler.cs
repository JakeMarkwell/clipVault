using clipVault.Models.Images.GetThumbnail;
using clipVault.Repositories.Images;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace clipVault.Handlers.Images
{
    public class GetThumbnailHandler : IRequestHandler<GetThumbnailRequest, GetThumbnailResponse>
    {
        private readonly IGetThumbnailService _thumbnailService;

        public GetThumbnailHandler(IGetThumbnailService thumbnailService)
        {
            _thumbnailService = thumbnailService;
        }

        public async Task<GetThumbnailResponse> Handle(GetThumbnailRequest request, CancellationToken cancellationToken)
        {
            return await _thumbnailService.GetThumbnailAsync(request.id, cancellationToken);
        }
    }
}