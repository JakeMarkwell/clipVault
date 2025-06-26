using MediatR;

namespace clipVault.Models.Images.GetThumbnails
{
    public class GetThumbnailsRequest : IRequest<GetThumbnailsResponse>
    {
        public int Limit { get; set; }
    }
}