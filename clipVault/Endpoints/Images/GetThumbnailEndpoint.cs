using clipVault.Models.Images.GetThumbnail;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.Metadata;
using System;

namespace clipVault.Endpoints.Images
{
    public class GetThumbnailEndpoint : Endpoint<GetThumbnailRequest, GetThumbnailDto>
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
            Validator<GetThumbnailRequestValidator>();
        }

        public override async Task HandleAsync(GetThumbnailRequest req, CancellationToken ct)
        {
            var response = await _mediator.Send(req, ct);

            if (response == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var result = new GetThumbnailDto
            {
                imageData = Convert.ToBase64String(response.imageData),
                fileType = response.fileType,
                title = response.title,
                friendTags = response.friendTags,
                categoryIds = response.categoryIds
            };

            await SendAsync(result, cancellation: ct);

        }
    }
}
