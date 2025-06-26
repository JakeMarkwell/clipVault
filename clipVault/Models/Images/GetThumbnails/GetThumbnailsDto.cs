using System.Collections.Generic;

namespace clipVault.Models.Images.GetThumbnails
{
    public class GetThumbnailsDto
    {
        public List<ThumbnailItemDto> Thumbnails { get; set; } = new List<ThumbnailItemDto>();
    }

    public class ThumbnailItemDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImageData { get; set; }
        public string FileType { get; set; }
        public string FriendTags { get; set; }
        public string CategoryTags { get; set; }
    }
}
