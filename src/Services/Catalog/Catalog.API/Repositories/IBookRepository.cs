using Catalog.API.Dtos;
using Catalog.Dtos;
using Catalog.Entities;

namespace Catalog.API.Repositories
{
    public interface IBookRepository
    {
        public Task<Book?> GetByIdAsync(string id);
        public Task CreateAsync(Book book);
        public Task UpdateAsync(Book book);
        public Task DeleteAsync(string id);
        public Task<GetPaginatedBooksDto> GetPaginatedBooksAsync(PaginationFormDto form);
        public Task<List<Book>> SearchBooksAsync(string keyword);
        public Task<List<Book>> GetLatestBooksAsync(int count = 10);
        public Task<List<Book>> GetDiscountedBooksAsync();
        public Task<List<Book>> GetTopRatedBooksAsync(int count = 10);
        public Task<bool> IsEmptyAsync();

    }
}
