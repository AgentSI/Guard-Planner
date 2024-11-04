using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email-ul este obligatoriu")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Parola este obligatorie")]
        public string Password { get; set; }
    }
}
