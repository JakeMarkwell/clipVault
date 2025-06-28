using Azure.Storage.Blobs;
using clipVault.Handlers.Images;
using clipVault.Repositories.Images;
using clipVault.Repositories.Video;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhostAngular", builder => 
    {
        builder.WithOrigins("http://localhost:51453") 
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFastEndpoints();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetThumbnailHandler).Assembly));

builder.Services.AddSingleton(new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage")));

//Repositories
builder.Services.AddTransient<IVideoRepository, VideoRepository>();
builder.Services.AddTransient<IThumbnailService, ThumbnailService>();

//Fluent Validators
builder.Services.AddValidatorsFromAssemblyContaining<UploadVideoRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetThumbnailRequestValidator>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 314572800; // 300MB in bytes
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 314572800; // 300MB in bytes
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

var app = builder.Build();

// Use CORS (Corrected)
app.UseCors("AllowLocalhostAngular");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseFastEndpoints();
app.Run();
