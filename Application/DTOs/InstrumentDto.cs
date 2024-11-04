using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class InstrumentDto
    {
        public Guid Id { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "Cantitatea trebuie să fie mai mare decât 0")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Numele este obligatoriu")]
        public string Name { get; set; }
        public Guid ReceiptId { get; set; }
    }
}
