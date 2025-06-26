using FluentValidation;
using clipVault.Models.Videos.UploadVideo;

public class UploadVideoRequestValidator : AbstractValidator<UploadVideoRequest>
{
    public UploadVideoRequestValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("No file attached")
            .Must(file => file.ContentType == "video/mp4").WithMessage("Only MP4 files are allowed.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Video title is required.");
    }
}
