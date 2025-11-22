using MongoDB.Driver;
using ShopHub.Models;

namespace ShopHub.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<ProductViewModel> _productsCollection;

        public MongoDbService(IConfiguration config)
        {
            var mongoClient = new MongoClient(config["MongoDB:ConnectionString"]);
            var mongoDatabase = mongoClient.GetDatabase(config["MongoDB:DatabaseName"]);

            _productsCollection = mongoDatabase.GetCollection<ProductViewModel>(config["MongoDB:CollectionName"]);
        }

        // 1. Get All Products (with optional filter)
        public async Task<List<ProductViewModel>> GetProductsAsync() =>
            await _productsCollection.Find(_ => true).ToListAsync();

        // 2. Get Single Product
        public async Task<ProductViewModel?> GetProductByIdAsync(string id) =>
            await _productsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // 3. Create Product (For seeding data)
        public async Task CreateProductAsync(ProductViewModel newProduct) =>
            await _productsCollection.InsertOneAsync(newProduct);

        // 4. Search Logic
        public async Task<List<ProductViewModel>> SearchProductsAsync(string searchTerm, string category)
        {
            // Start with all
            var builder = Builders<ProductViewModel>.Filter;
            var filter = builder.Empty;

            // If search term exists, filter by Name
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Regex for "Contains" (case insensitive)
                var regex = new MongoDB.Bson.BsonRegularExpression(searchTerm, "i");
                filter &= builder.Regex("Name", regex);
            }

            // If category exists, filter by Category
            if (!string.IsNullOrEmpty(category) && category != "All category")
            {
                filter &= builder.Eq("Category", category);
            }

            return await _productsCollection.Find(filter).ToListAsync();
        }
    }
}
