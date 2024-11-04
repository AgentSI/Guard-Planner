namespace Domain.Entities
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public string? Measure { get; set; }
        public Guid ReceiptId { get; set; }
        public Receipt Receipt { get; set; }
    }
}
