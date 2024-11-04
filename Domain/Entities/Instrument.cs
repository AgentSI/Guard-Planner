namespace Domain.Entities
{
    public class Instrument
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public string Name { get; set; }
        public Guid ReceiptId { get; set; }
        public Receipt Receipt { get; set; }
    }
}
