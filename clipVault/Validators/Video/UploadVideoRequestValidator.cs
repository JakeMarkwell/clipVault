using FluentValidation;
using clipVault.Models.Videos.UploadVideo;

public class UploadVideoRequestValidator : AbstractValidator<UploadVideoRequest>
{
    public UploadVideoRequestValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("No file attached")
            .Must(file => file.ContentType == "video/mp4").WithMessage("Only MP4 files are allowed.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Video name is required.");

        RuleFor(x => x.Tags)
            .MaximumLength(100).WithMessage("Tags cannot be longer than 100 characters");
    }
}
