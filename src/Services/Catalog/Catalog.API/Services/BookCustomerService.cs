using Catalog.API.Dtos;
using Catalog.API.GrpcServices;
using Catalog.API.Repositories;
using Catalog.API.Services;
using Catalog.Entities;


namespace Catalog.Application.Services
{
    public class BookCustomerService : IBookCustomerService
    {
        private readonly IBookRepository _repository;
        private readonly DiscountGrpcService _discountService;

        public BookCustomerService(
            IBookRepository repository,
            DiscountGrpcService discountService
            )
        {
            _repository = repository;
            _discountService = discountService;
          
        }

      


  

        public async Task<ApiResponseDto<BookGetDto>> GetByIdAsync(string id)
        {
            var book = await _repository.GetByIdAsync(id);
            if (book is null)
                return UApiResponderDto<BookGetDto>.NotFound("Book not found");


            var discount = await _discountService.GetDiscount(id);

            if ((decimal)discount.Amount > book.Price)
            {
                throw new Exception("Discount amount must be less or equal to product price");

            }

            var Book = new BookGetDto
            {
                Summary = book.Summary,
                Authors = book.Authors,
                AverageRating = book.AverageRating,
                Category = book.Category,
                Description = book.Description,
                DiscountAmount = (decimal)discount.Amount,
                Id = book.Id,
                ImageUrl = book.ImageUrl,
                Name = book.Name,
                PagesCount = book.PagesCount,
                Price = book.Price,
                PublishedAt = book.PublishedAt,
                Quantity=book.Quantity,
                RatingsCount = book.RatingsCount,
            };
            

               return UApiResponderDto<BookGetDto>.Ok(Book);
        }

        public async Task<ApiResponseDto<GetPaginatedBooksDto>> GetPaginatedBooksAsync(PaginationFormDto filter)
        {
            var books = await _repository.GetPaginatedBooksAsync(filter);

            await ApplyDiscounts(books.Books);

            return UApiResponderDto<GetPaginatedBooksDto>.Ok(books);
        }


        public async Task<ApiResponseDto<List<BookGetDto>>> SearchBooksAsync(string keyword)
        {
            var books = await _repository.SearchBooksAsync(keyword);
            var Books = MapBooks(books);

            await ApplyDiscounts(Books);


            return UApiResponderDto<List<BookGetDto>>.Ok(Books, $"Found {Books.Count} books matching '{keyword}'");
        }

        public async Task<ApiResponseDto<List<BookGetDto>>> GetTopRatedBooksAsync(int count = 10)
        {
            var books = await _repository.GetTopRatedBooksAsync(count);
            var Books = MapBooks(books);

            await ApplyDiscounts(Books);

            return UApiResponderDto<List<BookGetDto>>.Ok(Books, "Top rated books fetched");
        }

        public async Task<ApiResponseDto<List<BookGetDto>>> GetLatestBooksAsync(int count = 10)
        {
            var books = await _repository.GetLatestBooksAsync(count);
            var Books = MapBooks(books);

            await ApplyDiscounts(Books);

            return UApiResponderDto<List<BookGetDto>>.Ok(Books, "Latest books fetched");
        }

        

        public async Task<ApiResponseDto<List<BookGetDto>>> GetDiscountedBooksAsync()
        {
            var books = await _repository.GetDiscountedBooksAsync();
            var Books = MapBooks(books);

            await ApplyDiscounts(Books);

            return UApiResponderDto<List<BookGetDto>>.Ok(Books, "Discounted books fetched");
        }



       private async Task ApplyDiscounts(List<BookGetDto> books)
        {
            foreach (var book in books)
            {
                var discount = await _discountService.GetDiscount(book.Id);
                if ((decimal)discount.Amount > book.Price)
                {
                    throw new Exception("Discount amount must be less or equal to product price");

                }

                book.DiscountAmount = (decimal)discount.Amount;

            }

        }

        private List<BookGetDto> MapBooks(List<Book> books)
        {
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

            return Books;
        }

    }
}