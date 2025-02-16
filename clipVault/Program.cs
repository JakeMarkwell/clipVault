using Azure.Storage.Blobs;
using clipVault.Handlers.Images;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Features;

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
