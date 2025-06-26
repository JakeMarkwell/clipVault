using clipVault.Models.Images.GetThumbnails;
using FastEndpoints;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace clipVault.Endpoints.Images
{
    public class GetThumbnailsEndpoint : Endpoint<GetThumbnailsRequest, GetThumbnailsDto>
    {
        private readonly IMediator _mediator;

        public GetThumbnailsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/thumbnails");
            AllowAnonymous();
            Validator<GetThumbnailsRequestValidator>();
        }

        public override async Task HandleAsync(GetThumbnailsRequest req, CancellationToken ct)
        {
            var response = await _mediator.Send(req, ct); // Fixed 'response' declaration

            if (response == null || !response.Thumbnails.Any())
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var result = new GetThumbnailsDto
            {
                Thumbnails = response.Thumbnails.Select(t => new ThumbnailItemDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    ImageData = Convert.ToBase64String(t.ImageData),
                    FileType = t.FileType,
                    FriendTags = t.FriendTags,
                    CategoryTags = t.CategoryTags
                }).ToList()
            };

            await SendAsync(result, cancellation: ct);
        }
    }
}
