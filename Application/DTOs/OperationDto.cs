using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class OperationDto
    {
        public Guid Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Type { get; set; }
        [Required(ErrorMessage = "Ora de început este obligatorie")]
        public TimeSpan? StartTime { get; set; }
        [Required(ErrorMessage = "Ora de sfârșit este obligatorie")]
        public TimeSpan? EndTime { get; set; }
        public Guid GuardId { get; set; }
        public Guid ReceiptId { get; set; }
    }
}
