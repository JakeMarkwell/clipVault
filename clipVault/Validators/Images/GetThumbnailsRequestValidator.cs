using FluentValidation;

namespace clipVault.Models.Images.GetThumbnails
{
    public class GetThumbnailsRequestValidator : AbstractValidator<GetThumbnailsRequest>
    {
        public GetThumbnailsRequestValidator()
        {
            RuleFor(x => x.Limit).GreaterThan(0).LessThanOrEqualTo(100);
        }
    }
}
