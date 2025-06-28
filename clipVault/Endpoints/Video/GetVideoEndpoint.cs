using clipVault.Models.Video.GetVideo;
using FastEndpoints;
using MediatR;

namespace clipVault.Endpoints.Video
{
    public class GetVideoEndpoint : Endpoint<GetVideoRequest, GetVideoResponse>
    {
        private readonly IMediator _mediator;

        public GetVideoEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/video/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetVideoRequest req, CancellationToken ct)
        {
            var response = await _mediator.Send(req, ct);
            if (response == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }
            await SendAsync(response, cancellation: ct);
        }
    }
}
