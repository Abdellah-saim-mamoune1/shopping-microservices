namespace Catalog.Dtos
{
    public class BookDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Authors { get; set; } = new();
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int PagesCount { get; set; }
        public float averageRating { get; set; }
        public int ratingsCount { get; set; }

        public int Quantity { get; set; }
        public DateOnly? PublishedAt { get; set; }
    }
}
