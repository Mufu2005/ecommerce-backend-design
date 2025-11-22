using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShopHub.Models
{
    public class ProductViewModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public double Rating { get; set; }
        public int ReviewsCount { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public bool InStock { get; set; }
        public string SupplierName { get; set; }
    }
}
