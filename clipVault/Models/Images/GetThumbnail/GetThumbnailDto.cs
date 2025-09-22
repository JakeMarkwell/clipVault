public class GetThumbnailDto
{
    public string imageData { get; set; }
    public string fileType { get; set; }
    public string title { get; set; }
    public string friendTags { get; set; }
    public List<int> categoryIds { get; set; }
}
