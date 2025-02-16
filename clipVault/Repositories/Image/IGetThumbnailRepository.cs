namespace clipVault.Repositories.Images
{
    public interface IGetThumbnailRepository
    {
        Task<byte[]> GetThumbnailAsync(string id, CancellationToken cancellationToken);
    }
}
