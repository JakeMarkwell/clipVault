using MediatR;

namespace clipVault.Models.Images.GetThumbnail
{
    public class GetThumbnailRequest : IRequest<GetThumbnailResponse>
    {
        required public string id { get; set; }
    }
}
