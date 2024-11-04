namespace Domain.Entities
{
    public class AuthorizationVariables
    {
        // Password validity in days
        public static readonly int PasswordValidity = 30;

        // Salt used to encrypt password
        public static readonly string Salt = @"2:DPrL.25b.P]+B5";

        // Security code validity in seconds
        public static readonly double SecurityCodeValidity = TimeSpan.FromMinutes(5).TotalSeconds;

        public static readonly double CookieExpiration = TimeSpan.FromDays(1).TotalHours;
    }
}
