using System.Collections.Generic;

namespace clipVault.Models.Categories
{
    public class GetVideoCategoriesDto
    {
        public List<VideoCategoryDto> Categories { get; set; } = new();
    }

    public class VideoCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int Rating { get; set; }
        public string? ImageId { get; set; }
    }
}