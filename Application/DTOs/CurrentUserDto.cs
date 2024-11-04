namespace Application.DTOs
{
    public class CurrentUserDto
    {
        public bool IsAuthenticated { get; set; }
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserRole { get; set; }
    }
}
