namespace Discount.API.Dtos
{
    public class UApiResponderDto<T>
    {
        public static ApiResponseDto<T> Ok(T? data, string message = "Success", int status = 200)
    => new() { StatusCode = status, Success = true, Title = message, Data = data, Errors = default };


        public static ApiResponseDto<T> BadRequest(IEnumerable<ValidationErorrsDto>? Errors = null, string Title= "Invalid pieces of informatio")
     => new() { StatusCode = 400, Success = false, Title = Title, Errors = Errors, Data = default };

        public static ApiResponseDto<T> Unauthorized(string message = "Unauthorized")
     => new() { StatusCode = 401, Success = false, Title = "Invalid credintials" };

        public static ApiResponseDto<T> Forbidden(string message = "Forbidden", string? correlationId = null)
          => new()
          {
              StatusCode = 403,
              Success = false,
              Title = message,
              Errors = null,
              Data = default,

          };

        public static ApiResponseDto<T> InternalServerError()
   => new() { StatusCode = 500, Success = false, Title = "Internal server error" };



    }
}
