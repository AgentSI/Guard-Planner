using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public Guid UserRoleId { get; set; }
        public string? Role { get; set; }
        public string? Password { get; set; }
        [Required(ErrorMessage = "Email-ul este obligatoriu")]
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
