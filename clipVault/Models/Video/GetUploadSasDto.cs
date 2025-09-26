using FastEndpoints;
using FluentValidation;

public class GetUploadSasRequest
{
    public string FileName { get; set; } = string.Empty;
}

public class GetUploadSasRequestValidator : Validator<GetUploadSasRequest>
{
    public GetUploadSasRequestValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("FileName is required.")
            .MaximumLength(200).WithMessage("FileName too long.")
            .Matches(@"^[\w\-. ]+$").WithMessage("FileName contains invalid characters.");
    }
}

public class GetUploadSasResponse
{
    public string SasUrl { get; set; } = string.Empty;
}
