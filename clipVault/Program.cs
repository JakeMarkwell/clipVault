using Azure.Storage.Blobs;
using clipVault.Handlers.Images;
using clipVault.Repositories.Images;
using clipVault.Repositories.Video;
using clipVault.Scenarios.Images;
using clipVault.Scenarios.Video;
using clipVault.Services.Images;
using FastEndpoints;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
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
builder.Services.AddTransient<IGetThumbnailRepository, GetThumbnailRepository>();


//Scenarios
builder.Services.AddTransient<IGetThumbnailScenario, GetThumbnailScenario>();
builder.Services.AddTransient<IUploadVideoScenario, UploadVideoScenario>();
builder.Services.AddTransient<IDeleteVideoScenario, DeleteVideoScenario>();

//Services
builder.Services.AddTransient<IThumbnailGenerator, ThumbnailGenerator>();

//Fluent Validators
builder.Services.AddValidatorsFromAssemblyContaining<UploadVideoRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DeleteVideoScenario>();
builder.Services.AddValidatorsFromAssemblyContaining<GetThumbnailRequestValidator>();


builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 157286400; // 150MB in bytes
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigins");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseFastEndpoints();
app.Run();
