using System.Security.Claims;

namespace Application.DTOs
{
    public class ClaimsDto
    {
        public string UserId { get; set; } = "userId";
        public string Role { get; set; } = "role";
        public string Email { get; set; } = "email";

        public ClaimsDto(string userId, string role, string email)
        {
            UserId = userId;
            Role = role;
            Email = email;
        }
        public ClaimsDto() { }

        public ClaimsDto(IEnumerable<Claim> claims)
        {
            UserId = claims.Single(c => c.Type == "userId").Value;
            Role = claims.Single(c => c.Type == "role").Value;
            Email = claims.Single(c => c.Type == "email").Value;
        }

        public static string Id = "userId";
        public static string UserRole = "role";
        public static string UserEmail = "email";
    }
}
