using clipVault.Handlers.Video;
using clipVault.Models.Video.DeleteVideo;
using clipVault.Validators.Video;
using FastEndpoints;
using MediatR;

namespace clipVault.Endpoints.Videos
{
    public class DeleteVideoEndpoint : Endpoint<DeleteVideoRequest, DeleteVideoResponse>
    {
        private readonly IMediator _mediator;

        public DeleteVideoEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Delete("/videos/delete");
            AllowAnonymous();
            Validator<DeleteVideoRequestValidator>();
        }

        public override async Task HandleAsync(DeleteVideoRequest req, CancellationToken ct)
        {
            var response = await _mediator.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
