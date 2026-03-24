using Catalog.Entities;

namespace Catalog.API.Dtos
{
    public class GetPaginatedBooksDto
    {
        public List<BookGetDto> Books { get; set; } = new();
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
     
    }
}
