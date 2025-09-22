using clipVault.Models.Categories;
using clipVault.Models.Categories.GetVideoCategories;
using clipVault.Repositories.Categories;
using MediatR;

namespace clipVault.Handlers.Categories
{
    public class GetVideoCategoriesHandler : IRequestHandler<GetVideoCategoriesRequest, GetVideoCategoriesDto>
    {
        private readonly IVideoCategoryService _service;

        public GetVideoCategoriesHandler(IVideoCategoryService service)
        {
            _service = service;
        }

        public async Task<GetVideoCategoriesDto> Handle(GetVideoCategoriesRequest request, CancellationToken cancellationToken)
        {
            var categories = await _service.GetAllCategoriesAsync(cancellationToken);
            return new GetVideoCategoriesDto { Categories = categories };
        }
    }
}
