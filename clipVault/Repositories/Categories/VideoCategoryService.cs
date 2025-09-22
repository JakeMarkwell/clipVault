using Azure;
using Azure.Data.Tables;
using clipVault.Models.Categories;
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
    }
}
