using System.Collections.Generic;

namespace clipVault.Models.Images.GetThumbnails
{
    public class GetThumbnailsResponse
    {
        public List<ThumbnailItem> Thumbnails { get; set; } = new List<ThumbnailItem>();
    }
    
    public class ThumbnailItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public byte[] ImageData { get; set; }
        public string FileType { get; set; }
        public string FriendTags { get; set; }
        public string CategoryTags { get; set; }
    }
}
