namespace clipVault.Repositories.Video
{
    public interface IVideoRepository
    {
        Task UploadVideoAsync(IFormFile file, Dictionary<string, string> metadata, CancellationToken cancellationToken);
        Task<byte[]> GenerateThumbnailAsync(IFormFile file, CancellationToken cancellationToken);
    }
}
