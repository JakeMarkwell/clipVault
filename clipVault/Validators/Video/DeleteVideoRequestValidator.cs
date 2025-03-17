using clipVault.Models.Video.DeleteVideo;
using FluentValidation;

namespace clipVault.Validators.Video
{
    public class DeleteVideoRequestValidator : AbstractValidator<DeleteVideoRequest>
    {
        public DeleteVideoRequestValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage("Id cannot be empty.")
                .NotNull().WithMessage("Id cannot be null.");
        }
    }
}
