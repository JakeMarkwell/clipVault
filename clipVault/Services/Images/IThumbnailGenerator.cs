namespace clipVault.Services.Images
{
    public interface IThumbnailGenerator
    {
        Task<byte[]> GenerateThumbnailAsync(IFormFile file, CancellationToken cancellationToken);
    }
}