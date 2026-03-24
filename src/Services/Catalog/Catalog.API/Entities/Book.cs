using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Entities
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("Name")]
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Authors { get; set; } = new();
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int PagesCount { get; set; }
        public float AverageRating { get; set; }
        public int RatingsCount { get; set; }

        public int Quantity { get; set; }
        public DateOnly? PublishedAt { get; set; }

    }
}
