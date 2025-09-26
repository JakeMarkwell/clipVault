using clipVault.Models.Categories.AddVideoCategory;
using clipVault.Validators.Categories;
using FastEndpoints;
using MediatR;

namespace clipVault.Endpoints.Categories
{
    public class AddVideoCategoryEndpoint : Endpoint<AddVideoCategoryRequest, AddVideoCategoryResponse>
    {
        private readonly IMediator _mediator;

        public AddVideoCategoryEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/video-categories/add");
            AllowAnonymous();
            Validator<AddVideoCategoryRequestValidator>();
        }

        public override async Task HandleAsync(AddVideoCategoryRequest req, CancellationToken ct)
        {
            var response = await _mediator.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}