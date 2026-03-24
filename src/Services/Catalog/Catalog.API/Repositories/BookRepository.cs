using Catalog.API.Dtos;
using Catalog.API.Repositories;
using Catalog.Entities;
using Catalog.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IMongoCollection<Book> _books;

        public BookRepository(IMongoClient client, IOptions<MongoDbSettings> options)
        {
            var database = client.GetDatabase(options.Value.DatabaseName);
            _books = database.GetCollection<Book>("Products");
        }

        public async Task<Book?> GetByIdAsync(string id) =>
            await _books.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Book book) =>
            await _books.InsertOneAsync(book);

        public async Task UpdateAsync(Book book) =>
            await _books.ReplaceOneAsync(x => x.Id == book.Id, book);

        public async Task DeleteAsync(string id) =>
            await _books.DeleteOneAsync(x => x.Id == id);

        public async Task<GetPaginatedBooksDto> GetPaginatedBooksAsync(PaginationFormDto filter)
        {
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            var totalCount = await _books.CountDocumentsAsync(Builders<Book>.Filter.Empty);

            var books = await _books
                .Find(Builders<Book>.Filter.Empty)
                .Skip(skip)
                .Limit(filter.PageSize)
                .ToListAsync();

            var Books = new List<BookGetDto>() { };

            foreach (var book in books)
            {
                var Book = new BookGetDto
                {
                    Summary = book.Summary,
                    Authors = book.Authors,
                    AverageRating = book.AverageRating,
                    Category = book.Category,
                    Description = book.Description,
                    Id = book.Id,
                    ImageUrl = book.ImageUrl,
                    Name = book.Name,
                    PagesCount = book.PagesCount,
                    Price = book.Price,
                    PublishedAt = book.PublishedAt,
                    RatingsCount = book.RatingsCount,
                    Quantity = book.Quantity,
                };

                Books.Add(Book);
            }


            return new GetPaginatedBooksDto
            {
                Books = Books,
                TotalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }



        public async Task<List<Book>> SearchBooksAsync(string keyword)
        {
            var filter = Builders<Book>.Filter.Or(
                Builders<Book>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(keyword, "i")),
                Builders<Book>.Filter.Regex(x => x.Category, new MongoDB.Bson.BsonRegularExpression(keyword, "i")),
                Builders<Book>.Filter.Regex(x => x.Description, new MongoDB.Bson.BsonRegularExpression(keyword, "i"))
            );

            return await _books.Find(filter).ToListAsync();
        }

      
        public async Task<List<Book>> GetTopRatedBooksAsync(int count = 10)
        {
            return await _books
                .Find(Builders<Book>.Filter.Empty)
                .SortByDescending(x => x.AverageRating)
                .Limit(count)
                .ToListAsync();
        }

        
        public async Task<List<Book>> GetLatestBooksAsync(int count = 10)
        {
            return await _books
                .Find(Builders<Book>.Filter.Empty)
                .SortByDescending(x => x.PublishedAt)
                .Limit(count)
                .ToListAsync();
        }

       
      

        public async Task<List<Book>> GetDiscountedBooksAsync()
        {
            return await _books
                .Find(x => x.Price > 0 && x.AverageRating > 0) 
                .ToListAsync();
        }




        public async Task<bool> IsEmptyAsync()
        {
            return ! await _books.Find(_ => true).AnyAsync();
            
        }

    }
}