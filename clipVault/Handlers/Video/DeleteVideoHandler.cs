using clipVault.Models.Video.DeleteVideo;
using clipVault.Scenarios.Video;
using MediatR;

namespace clipVault.Handlers.Video
{
    public class DeleteVideoHandler : IRequestHandler<DeleteVideoRequest, DeleteVideoResponse>
    {
        private readonly IDeleteVideoScenario _deleteVideoScenario;

        public DeleteVideoHandler(IDeleteVideoScenario deleteVideoScenario)
        {
            _deleteVideoScenario = deleteVideoScenario;
        }

        public async Task<DeleteVideoResponse> Handle(DeleteVideoRequest request, CancellationToken cancellationToken)
        {
            return await _deleteVideoScenario.ExecuteAsync(request, cancellationToken);
        }
    }
}
