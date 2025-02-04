using clipVault.MockData;
using clipVault.Models.Images.GetThumbnail;
using MediatR;

namespace clipVault.Handlers.Images
{
    public class GetThumbnailHandler : IRequestHandler<GetThumbnailRequest, GetThumbnailResponse>
    {
        public async Task<GetThumbnailResponse> Handle(GetThumbnailRequest request, CancellationToken cancellationToken)
        {
            var mockImage = MockImage.MockImageArray.FirstOrDefault(img => img.id == request.id);

            return new GetThumbnailResponse
            {
                imageData= mockImage.imageData,
                fileType = mockImage.fileType
            };
        }
    }
}
