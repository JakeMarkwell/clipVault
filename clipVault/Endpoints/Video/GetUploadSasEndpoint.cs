using FastEndpoints;
using clipVault.Models.Video;
using clipVault.Handlers.Video;

public class GetUploadSasEndpoint : Endpoint<GetUploadSasRequest, GetUploadSasResponse>
{
    private readonly GetUploadSasHandler _sasHandler;

    public GetUploadSasEndpoint(GetUploadSasHandler sasHandler)
    {
        _sasHandler = sasHandler;
    }

    public override void Configure()
    {
        Get("/api/video/get-upload-sas");
        AllowAnonymous();
        Validator<GetUploadSasRequestValidator>();
    }

    public override async Task HandleAsync(GetUploadSasRequest req, CancellationToken ct)
    {
        var sasUrl = _sasHandler.GenerateSasUrl(req.FileName);
        await SendAsync(new GetUploadSasResponse { SasUrl = sasUrl });
    }
}
