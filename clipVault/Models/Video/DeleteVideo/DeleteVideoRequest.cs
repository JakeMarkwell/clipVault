using MediatR;

namespace clipVault.Models.Video.DeleteVideo
{
    public class DeleteVideoRequest : IRequest<DeleteVideoResponse>
    {
        public string Id { get; set; } = string.Empty;
    }
}
