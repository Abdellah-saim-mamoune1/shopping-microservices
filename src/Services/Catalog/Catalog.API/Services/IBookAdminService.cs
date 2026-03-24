using Catalog.API.Dtos;
using Catalog.Dtos;
using Catalog.Entities;

namespace Catalog.API.Services
{
    public interface IBookAdminService
    {
        public Task<ApiResponseDto<object>> CreateAsync(BookDto book);
        public Task<ApiResponseDto<object>> UpdateAsync(string Id, BookDto book);
        public Task<ApiResponseDto<object>> DeleteAsync(string BookId);
    }
}
