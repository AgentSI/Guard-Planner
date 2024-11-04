namespace Application.DTOs
{
    public class VacationCreateResultDto
    {
        public bool Success { get; set; }
        public Guid Id { get; set; } = Guid.Empty;
        public string? ErrorMessage { get; set; }
    }
}
