using System.ComponentModel.DataAnnotations;

namespace Authentication.API.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public string HashedRefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpirationDate { get; set; }

    }
}
