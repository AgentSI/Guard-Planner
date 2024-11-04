using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class WorkerDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Numele este obligatoriu")]
        public string Name { get; set; }
        public string? FirstName { get; set; }
        public string? Specialization { get; set; }
        [Required(ErrorMessage = "Email-ul este obligatoriu")]
        public string Email { get; set; }
        public bool Available { get; set; }
        public int NoDaysVacation { get; set; } = 35;
        public bool IsGuard { get; set; }
        public double Percent { get; set; }
        public List<GuardDto>? Guards { get; set; }
        public List<WorkerHoursDto>? NoHours { get; set; }
    }
}
