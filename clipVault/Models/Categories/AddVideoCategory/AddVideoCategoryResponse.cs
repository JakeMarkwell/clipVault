namespace clipVault.Models.Categories.AddVideoCategory
{
    public class AddVideoCategoryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}