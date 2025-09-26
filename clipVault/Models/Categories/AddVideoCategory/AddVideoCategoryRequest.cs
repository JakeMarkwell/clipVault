using MediatR;

namespace clipVault.Models.Categories.AddVideoCategory
{
    public class AddVideoCategoryRequest : IRequest<AddVideoCategoryResponse>
    {
        public string CategoryName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? ImageId { get; set; }
    }
}