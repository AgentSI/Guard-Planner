using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class VacationDto
    {
        public Guid Id { get; set; }
        public Guid WorkerId { get; set; }
        [Required(ErrorMessage = "Lucrător-ul este obligatoriu")]
        public string WorkerName { get; set; }
        [Required(ErrorMessage = "Data este obligatorie")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "Data este obligatorie")]
        public DateTime? EndDate { get; set; }
        public string? Reason { get; set; }
        public int NoDays { get; set; }
    }
}
