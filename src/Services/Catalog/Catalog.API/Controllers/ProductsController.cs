using Catalog.API.Dtos;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
   
    [Route("api/v1/catalog")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IBookCustomerService _bookService;

        public ProductsController(IBookCustomerService bookService)
        {
            _bookService = bookService;
        }
      

        [HttpGet("{PageNumber},{PageSize}")]
        public async Task<IActionResult> GetPaginatedBooks(int PageSize, int PageNumber)
        {
            var Form = new PaginationFormDto { PageSize = PageSize, PageNumber = PageNumber };
            var result = await _bookService.GetPaginatedBooksAsync(Form);

            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("by-id/{Id}")]
        public async Task<IActionResult> GetBookById(string Id)
        {
            var result = await _bookService.GetByIdAsync(Id);
            return StatusCode(result.StatusCode,result);

        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Keyword is required");

            var result = await _bookService.SearchBooksAsync(keyword);
            return StatusCode(result.StatusCode, result);
        }

        // Top-rated books
        [HttpGet("top-rated")]
        public async Task<IActionResult> GetTopRatedBooks([FromQuery] int count = 10)
        {
            var result = await _bookService.GetTopRatedBooksAsync(count);
            return StatusCode(result.StatusCode, result);
        }

      
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestBooks([FromQuery] int count = 10)
        {
            var result = await _bookService.GetLatestBooksAsync(count);
            return StatusCode(result.StatusCode, result);
        }

       
    
        
        [HttpGet("discounted")]
        public async Task<IActionResult> GetDiscountedBooks()
        {
            var result = await _bookService.GetDiscountedBooksAsync();
            return StatusCode(result.StatusCode, result);
        }

       
        [HttpGet("filter")]
        public async Task<IActionResult> FilterBooks(
            [FromQuery] string keyword,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            
            var books = await _bookService.SearchBooksAsync(keyword);

            if (books.Data == null)
                return Ok();

            var paginated = books.Data
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new GetPaginatedBooksDto
            {
                Books = paginated,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)(books.Data?.Count ?? 0) / pageSize)
            };

            return Ok(response);
        }

    }
}
