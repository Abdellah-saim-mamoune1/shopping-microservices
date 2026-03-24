namespace Authentication.API.Dtos
{
    public class AuthResponseDto
    {
       public int Id {  get; set; }
       public string Role { get; set; } = string.Empty;
       public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; } 
    }
}
