using FluentValidation;
using clipVault.Models.Images.GetThumbnail;

public class GetThumbnailRequestValidator : AbstractValidator<GetThumbnailRequest>
{
    public GetThumbnailRequestValidator()
    {
        RuleFor(x => x.id)
            .NotEmpty().WithMessage("No id provided")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("ID is not a valid GUID");
    }
}
