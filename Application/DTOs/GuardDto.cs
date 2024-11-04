using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class GuardDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Data este obligatorie")]
        public DateTime? Date { get; set; }
        public int Hours { get; set; }
        public Guid WorkerId { get; set; }
        [Required(ErrorMessage = "Lucrător-ul este obligatoriu")]
        public string WorkerName { get; set; }
        public List<OperationDto>? Operations { get; set; }
        public int NrOperations { get; set; }
        public string? Color { get; set; }
    }
}
