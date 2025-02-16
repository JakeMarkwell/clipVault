using clipVault.Models.Videos.UploadVideo;
using clipVault.Scenarios.Video;
using MediatR;

namespace clipVault.Handlers.Videos
{
    public class UploadVideoHandler : IRequestHandler<UploadVideoRequest, UploadVideoResponse>
    {
        private readonly IUploadVideoScenario _uploadVideoScenario;

        public UploadVideoHandler(IUploadVideoScenario uploadVideoScenario)
        {
            _uploadVideoScenario = uploadVideoScenario;
        }

        public async Task<UploadVideoResponse> Handle(UploadVideoRequest request, CancellationToken cancellationToken)
        {
           
            return await _uploadVideoScenario.ExecuteAsync(request, cancellationToken);
            
           
        }
    }
}
