using Catalog.API.Dtos;
using Catalog.API.Repositories;
using Catalog.API.Validators;
using Catalog.Dtos;
using Catalog.Entities;
using FluentValidation;

namespace Catalog.API.Services
{
    public class BookAdminService : IBookAdminService
    {
        private readonly IBookRepository _repository;
        
     

        public BookAdminService(IBookRepository repository)
        {
            _repository = repository;
           
        }

      

        public async Task<ApiResponseDto<object>> CreateAsync(BookDto dto)
        {
          
            var validator = new BookDtoValidator();

            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => new ValidationErorrsDto
                {
                    FieldId = e.PropertyName,
                    Message = e.ErrorMessage
                }).ToList();
                return UApiResponderDto<object>.BadRequest(errors);
            }

            var entity = new Book
            {
              
                Name = dto.Name,
                Category = dto.Category,
                Summary = dto.Summary,
                Description = dto.Description,
                Authors = dto.Authors,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                PagesCount = dto.PagesCount,
                AverageRating = dto.averageRating,
                RatingsCount = dto.ratingsCount,
                PublishedAt = dto.PublishedAt,
                Quantity = dto.Quantity,

            };

            await _repository.CreateAsync(entity);
            return UApiResponderDto<object>.Ok(null);
        }

        public async Task<ApiResponseDto<object>> UpdateAsync(string id, BookDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
                return UApiResponderDto<object>.NotFound("Book not found");

           

            var validator = new BookDtoValidator();
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => new ValidationErorrsDto
                {
                    FieldId = e.PropertyName,
                    Message = e.ErrorMessage
                }).ToList();
                return UApiResponderDto<object>.BadRequest(errors);
            }


            var entity = new Book
            {
                Id = id,
                Name = dto.Name,
                Category = dto.Category,
                Summary = dto.Summary,
                Description = dto.Description,
                Authors = dto.Authors,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                PagesCount = dto.PagesCount,
                AverageRating = dto.averageRating,
                RatingsCount = dto.ratingsCount,
                PublishedAt = dto.PublishedAt,
                Quantity = dto.Quantity,
            };
            await _repository.UpdateAsync(entity);
            return UApiResponderDto<object>.Ok(null);
        }

        public async Task<ApiResponseDto<object>> DeleteAsync(string id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
                return UApiResponderDto<object>.NotFound("Book not found");

            await _repository.DeleteAsync(id);
            return UApiResponderDto<object>.Ok(null);
        }
    }
}
