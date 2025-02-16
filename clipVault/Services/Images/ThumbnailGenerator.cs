using NReco.VideoConverter;

namespace clipVault.Services.Images
{
    public class ThumbnailGenerator : IThumbnailGenerator
    {
        public async Task<byte[]> GenerateThumbnailAsync(IFormFile file, CancellationToken cancellationToken)
        {
            var ffmpeg = new FFMpegConverter();
            using (var inputStream = file.OpenReadStream())
            using (var outputStream = new MemoryStream())
            {
                var tempFilePath = Path.GetTempFileName();
                using (var tempFileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    await inputStream.CopyToAsync(tempFileStream, cancellationToken);
                }
                ffmpeg.GetVideoThumbnail(tempFilePath, outputStream, 5);
                File.Delete(tempFilePath);
                return outputStream.ToArray();
            }
        }
    }
}
