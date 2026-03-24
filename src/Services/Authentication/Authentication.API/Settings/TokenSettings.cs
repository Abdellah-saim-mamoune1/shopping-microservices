namespace Authentication.API.Settings
{
    public static class TokenSettings
    {

        public  static DateTime TokenExpirationDate()
        {
            return DateTime.UtcNow.AddMinutes(10);
        }

        public static DateTime RefreshTokenExpirationDate()
        {
            return DateTime.UtcNow.AddHours(30);
        }
    }
}
