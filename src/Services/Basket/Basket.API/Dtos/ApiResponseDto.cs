namespace Basket.API.Dtos
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string? Title { get; set; }
        public T? Data { get; set; }
        public IEnumerable<ValidationErorrsDto>? Errors { get; set; }


    }

}
