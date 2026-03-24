using Catalog.API.Repositories;
using Catalog.Entities;

namespace Catalog.API.Seed
{
    public  class Seeder
    {

        private  readonly IBookRepository _repository;
        public  Seeder(IBookRepository repository)
        {
            _repository = repository;

        }


        public async Task SeedAsync()
        {
        
            if (!await _repository.IsEmptyAsync())
                return;


            var books = GetPreconfiguredBooks();

            foreach (var book in books)
            {
                await _repository.CreateAsync(book);
            }
        }




        private static List<Book> GetPreconfiguredBooks()
        { 
            var books = new List<Book>
{
    new Book
    {
        Name = "Clean Code",
        Category = "Programming",
        Summary = "A guide to writing clean, maintainable code.",
        Description = "Robert C. Martin explains principles and best practices for writing readable and efficient code.",
        Authors = new() { "Robert C. Martin" },
        ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41SH-SvWPxL.jpg",
        Price = 39.99m,
        PagesCount = 464,
        AverageRating = 4.7f,
        RatingsCount = 50000,
        Quantity=10,
        PublishedAt = new DateOnly(2008, 8, 1)
    },
    new Book
    {
        Name = "The Pragmatic Programmer",
        Category = "Programming",
        Summary = "Best practices for modern software development.",
        Description = "Covers tips and philosophy for becoming a better developer.",
        Authors = new() { "Andrew Hunt", "David Thomas" },
        ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41as+WafrFL.jpg",
        Price = 42.50m,
        PagesCount = 352,
        AverageRating = 4.8f,
        RatingsCount = 42000,
        Quantity=10,
        PublishedAt = new DateOnly(1999, 10, 20)
    },
  
    new Book
    {
        Name = "Atomic Habits",
        Category = "Self-help",
        Summary = "Small habits lead to big results.",
        Description = "James Clear explains how tiny changes build powerful habits.",
        Authors = new() { "James Clear" },
        ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/51-uspgqWIL.jpg",
        Price = 24.99m,
        PagesCount = 320,
        AverageRating = 4.8f,
        RatingsCount = 90000,
        Quantity=4,
        PublishedAt = new DateOnly(2018, 10, 16)
    },
   
    new Book
    {
        Name = "Rich Dad Poor Dad",
        Category = "Finance",
        Summary = "Financial education basics.",
        Description = "Teaches mindset and financial independence principles.",
        Authors = new() { "Robert T. Kiyosaki" },
        ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/51AHZGhzZEL.jpg",
        Price = 14.99m,
        PagesCount = 336,
        AverageRating = 4.7f,
        RatingsCount = 80000,
        Quantity=10,
        PublishedAt = new DateOnly(1997, 4, 1)
    },
  
    new Book
    {
        Name = "To Kill a Mockingbird",
        Category = "Fiction",
        Summary = "Justice and racism in the American South.",
        Description = "A young girl learns moral courage through her father.",
        Authors = new() { "Harper Lee" },
        ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/51IXWZzlgSL.jpg",
        Price = 10.99m,
        PagesCount = 281,
        AverageRating = 4.8f,
        RatingsCount = 95000,
        Quantity=10,
        PublishedAt = new DateOnly(1960, 7, 11)
    },
    new Book
    {
        Name = "The Hobbit",
        Category = "Fantasy",
        Summary = "A hobbit’s adventure.",
        Description = "Bilbo Baggins goes on a quest with dwarves.",
        Authors = new() { "J.R.R. Tolkien" },
        ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41aQPTCmeVL.jpg",
        Price = 12.99m,
        PagesCount = 310,
        AverageRating = 4.8f,
        RatingsCount = 110000,
        Quantity=10,
        PublishedAt = new DateOnly(1937, 9, 21)
    },
    new Book
    {
        Name = "Harry Potter and the Sorcerer's Stone",
        Category = "Fantasy",
        Summary = "A young wizard begins his journey.",
        Description = "Harry discovers Hogwarts and magic.",
        Authors = new() { "J.K. Rowling" },
        ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/51UoqRAxwEL.jpg",
        Price = 11.99m,
        PagesCount = 309,
        AverageRating = 4.9f,
        RatingsCount = 150000,
        Quantity=10,
        PublishedAt = new DateOnly(1997, 6, 26)
    },
    new Book
    {
        Name = "The Alchemist",
        Category = "Fiction",
        Summary = "Follow your dreams.",
        Description = "A shepherd travels in search of treasure.",
        Authors = new() { "Paulo Coelho" },
        ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41ybG235TcL.jpg",
        Price = 13.99m,
        PagesCount = 208,
        AverageRating = 4.7f,
        RatingsCount = 85000,
        Quantity=10,
        PublishedAt = new DateOnly(1988, 1, 1)
    },
    new Book
    {
        Name = "Zero to One",
        Category = "Business",
        Summary = "Building innovative startups.",
        Description = "Focus on creating unique value.",
        Authors = new() { "Peter Thiel" },
        ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41uPjEenkFL.jpg",
        Price = 17.99m,
        PagesCount = 224,
        AverageRating = 4.5f,
        RatingsCount = 30000,
        Quantity=12,
        PublishedAt = new DateOnly(2014, 9, 16)
    }
};
            return books;

        }


    }
}
