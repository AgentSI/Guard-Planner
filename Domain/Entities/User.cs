namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public Guid UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
        public string? PasswordHash { get; set; }
        public string Email { get; set; }
        public string? SecurityCode { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
