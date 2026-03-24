using Catalog.API.Dtos;
using Catalog.Dtos;
using Catalog.Entities;



namespace Catalog.API.Services
{
    public interface IBookCustomerService
    {
       
        public Task<ApiResponseDto<BookGetDto>> GetByIdAsync(string id);
        public Task<ApiResponseDto<GetPaginatedBooksDto>> GetPaginatedBooksAsync(PaginationFormDto filter);
        public Task<ApiResponseDto<List<BookGetDto>>> SearchBooksAsync(string keyword);
        public Task<ApiResponseDto<List<BookGetDto>>> GetTopRatedBooksAsync(int count = 10);
        public Task<ApiResponseDto<List<BookGetDto>>> GetLatestBooksAsync(int count = 10);
        public Task<ApiResponseDto<List<BookGetDto>>> GetDiscountedBooksAsync();

    }
}
