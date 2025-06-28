using MediatR;

namespace clipVault.Models.Video.GetVideo
{
    public class GetVideoRequest : IRequest<GetVideoResponse>
    {
        public required string Id { get; set; }
    }
}
