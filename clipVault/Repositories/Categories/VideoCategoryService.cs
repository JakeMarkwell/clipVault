using Azure;
using Azure.Data.Tables;
using clipVault.Models.Categories;
using clipVault.Models.Categories.AddVideoCategory;
using clipVault.Models.Categories.GetVideoCategories;
using Microsoft.Extensions.Configuration;

namespace clipVault.Repositories.Categories
{
    public class VideoCategoryService : IVideoCategoryService
    {
        private readonly TableClient _tableClient;

        public VideoCategoryService(IConfiguration configuration)
        {
            var accountName = configuration["AzureTable:AccountName"];
            var accountKey = configuration["AzureTable:AccountKey"];
            var tableName = configuration["AzureTable:TableName"];
            var serviceUri = $"https://{accountName}.table.core.windows.net";
            var credential = new TableSharedKeyCredential(accountName, accountKey);
            _tableClient = new TableClient(new Uri(serviceUri), tableName, credential);
        }

        public async Task<List<VideoCategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken)
        {
            var categories = new List<VideoCategoryDto>();
            await foreach (var entity in _tableClient.QueryAsync<TableEntity>(cancellationToken: cancellationToken))
            {
                categories.Add(new VideoCategoryDto
                {
                    Id = int.Parse(entity.RowKey),
                    CategoryName = (entity.GetString("categoryName") ?? "").Trim('"'),
                    Rating = entity.GetInt32("rating") ?? 0,
                    ImageId = entity.ContainsKey("imageId") ? entity.GetString("imageId") : null
                });
            }
            return categories;
        }

        public async Task<AddVideoCategoryResponse> AddVideoCategoryAsync(AddVideoCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Generate a new category ID by finding the maximum existing ID and adding 1
                var existingCategories = await GetAllCategoriesAsync(cancellationToken);
                var nextId = existingCategories.Any() ? existingCategories.Max(c => c.Id) + 1 : 1;

                // Create the table entity
                var entity = new TableEntity("CATEGORY", nextId.ToString())
                {
                    ["categoryName"] = $"\"{request.CategoryName}\"",
                    ["rating"] = request.Rating,
                    ["imageId"] = "NULL"
                };

                // Add imageId if provided
                if (!string.IsNullOrEmpty(request.ImageId))
                {
                    entity["imageId"] = request.ImageId;
                }

                // Insert the entity into the table
                await _tableClient.AddEntityAsync(entity, cancellationToken);

                return new AddVideoCategoryResponse
                {
                    Success = true,
                    Message = "Video category added successfully",
                    CategoryId = nextId
                };
            }
            catch (RequestFailedException ex)
            {
                return new AddVideoCategoryResponse
                {
                    Success = false,
                    Message = $"Failed to add video category: {ex.Message}",
                    CategoryId = 0
                };
            }
            catch (Exception ex)
            {
                return new AddVideoCategoryResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    CategoryId = 0
                };
            }
        }
    }
}
