using clipVault.Models.Categories.AddVideoCategory;
using clipVault.Repositories.Categories;
using MediatR;

namespace clipVault.Handlers.Categories
{
    public class AddVideoCategoryHandler : IRequestHandler<AddVideoCategoryRequest, AddVideoCategoryResponse>
    {
        private readonly IVideoCategoryService _service;

        public AddVideoCategoryHandler(IVideoCategoryService service)
        {
            _service = service;
        }

        public async Task<AddVideoCategoryResponse> Handle(AddVideoCategoryRequest request, CancellationToken cancellationToken)
        {
            return await _service.AddVideoCategoryAsync(request, cancellationToken);
        }
    }
}