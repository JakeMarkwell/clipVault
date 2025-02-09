using clipVault.Models.Videos.UploadVideo;
using FastEndpoints;
using MediatR;

namespace clipVault.Endpoints.Videos
{
    public class UploadVideoEndpoint : Endpoint<UploadVideoRequest, UploadVideoResponse>
    {
        private readonly IMediator _mediator;

        public UploadVideoEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/videos/upload");
            AllowAnonymous();
            //Need this to allow mp4 files etc.
            AllowFileUploads();
        }

        public override async Task HandleAsync(UploadVideoRequest req, CancellationToken ct)
        {
            var response = await _mediator.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
