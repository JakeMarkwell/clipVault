using MediatR;

namespace clipVault.Models.Images.GetThumbnail
{
    public class GetThumbnailRequest : IRequest<GetThumbnailResponse>
    {
        public string id { get; set; }
    }
}
