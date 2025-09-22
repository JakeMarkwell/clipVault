using clipVault.Models.Categories;
using clipVault.Models.Categories.GetVideoCategories;
using FastEndpoints;
using MediatR;

namespace clipVault.Endpoints.Categories
{
    public class GetVideoCategoriesEndpoint : Endpoint<EmptyRequest, GetVideoCategoriesDto>
    {
        private readonly IMediator _mediator;

        public GetVideoCategoriesEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/video-categories");
            AllowAnonymous();
        }

        public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            var response = await _mediator.Send(new GetVideoCategoriesRequest(), ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
