using Catalog.API.Services;
using Catalog.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    //[Authorize(Roles ="Admin")]
    [Route("api/v1/catalog/admin")]
    [ApiController]

    //port: 8000
    public class ProductsAdminController : ControllerBase
    {
        private readonly IBookAdminService _bookService;

        public ProductsAdminController(IBookAdminService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(BookDto book)
        {
            var data = await _bookService.CreateAsync(book);
                return StatusCode(data.StatusCode, data);
        }

        [HttpPut("{BookId}")]
        public async Task<IActionResult> UpdateBook(string BookId, BookDto book)
        {
            var data = await _bookService.UpdateAsync(BookId, book);
                return StatusCode(data.StatusCode, data);
        }

        [HttpDelete("{BookId}")]
        public async Task<IActionResult> DeleteBook(string BookId)
        {
            var data = await _bookService.DeleteAsync(BookId);
                return StatusCode(data.StatusCode, data);
        }
    }

}

