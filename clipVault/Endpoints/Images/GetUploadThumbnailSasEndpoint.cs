using FastEndpoints;
using clipVault.Models.Images;
using clipVault.Handlers.Images;

public class GetUploadThumbnailSasEndpoint : Endpoint<GetUploadThumbnailSasRequest, GetUploadThumbnailSasResponse>
{
    private readonly GetUploadThumbnailSasHandler _handler;

    public GetUploadThumbnailSasEndpoint(GetUploadThumbnailSasHandler handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Get("/api/image/get-upload-sas");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetUploadThumbnailSasRequest req, CancellationToken ct)
    {
        var sasUrl = _handler.GenerateSasUrl(req.FileName);
        await SendAsync(new GetUploadThumbnailSasResponse { SasUrl = sasUrl });
    }
}