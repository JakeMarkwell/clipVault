using clipVault.Models.Images.GetThumbnail;
using FastEndpoints;
using MediatR;

namespace clipVault.Endpoints.Images
{
    public class GetThumbnailEndpoint : Endpoint<GetThumbnailRequest, GetThumbnailResponse>
    {
        private readonly IMediator _mediator;

        public GetThumbnailEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/thumbnail/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetThumbnailRequest req, CancellationToken ct)
        {
            var response = await _mediator.Send(req, ct);

            HttpContext.Response.ContentType = "image/png";
            await HttpContext.Response.BodyWriter.WriteAsync(response.imageData, ct);
        }
    }
}
