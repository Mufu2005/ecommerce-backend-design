using MongoDB.Driver;
using ShopHub.Models;

namespace ShopHub.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<ProductViewModel> _productsCollection;
        private readonly IMongoCollection<User> _usersCollection;

        public MongoDbService(IConfiguration config)
        {
            var mongoClient = new MongoClient(config["MongoDB:ConnectionString"]);
            var mongoDatabase = mongoClient.GetDatabase(config["MongoDB:DatabaseName"]);

            _productsCollection = mongoDatabase.GetCollection<ProductViewModel>(config["MongoDB:CollectionName"]);
            _usersCollection = mongoDatabase.GetCollection<User>("Users");
        }

        public async Task<(List<ProductViewModel> products, long totalCount)> GetProductsAsync(int page, int pageSize)
        {
            var filter = Builders<ProductViewModel>.Filter.Empty;

            // Count all items first (for calculating total pages)
            var totalCount = await _productsCollection.CountDocumentsAsync(filter);

            // Get specific chunk
            var products = await _productsCollection.Find(filter)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }

        public async Task<ProductViewModel?> GetProductByIdAsync(string id) =>
            await _productsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateProductAsync(ProductViewModel newProduct) =>
            await _productsCollection.InsertOneAsync(newProduct);

        public async Task<List<ProductViewModel>> SearchProductsAsync(string searchTerm, string category)
        {
            var builder = Builders<ProductViewModel>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var regex = new MongoDB.Bson.BsonRegularExpression(searchTerm, "i");
                filter &= builder.Regex("Name", regex);
            }

            if (!string.IsNullOrEmpty(category) && category != "All category")
            {
                filter &= builder.Eq("Category", category);
            }

            return await _productsCollection.Find(filter).ToListAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _usersCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
        }
       
    }
}
