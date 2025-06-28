namespace clipVault.Models.Video.GetVideo
{
    public class GetVideoResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string CategoryTags { get; set; }
        public string FriendTags { get; set; }
        public string VideoUrl { get; set; }
        public string ContentType { get; set; } // e.g., "video/mp4"
    }
}
