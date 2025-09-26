using clipVault.Models.Categories.AddVideoCategory;
using FastEndpoints;
using FluentValidation;

namespace clipVault.Validators.Categories
{
    public class AddVideoCategoryRequestValidator : Validator<AddVideoCategoryRequest>
    {
        public AddVideoCategoryRequestValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");

            RuleFor(x => x.Rating)
                .GreaterThanOrEqualTo(0).WithMessage("Rating must be 0 or greater.")
                .LessThanOrEqualTo(10).WithMessage("Rating cannot exceed 10.");

            RuleFor(x => x.ImageId)
                .MaximumLength(500).WithMessage("Image ID cannot exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.ImageId));
        }
    }
}