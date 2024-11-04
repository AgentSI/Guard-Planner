namespace Domain.Entities
{
    public class Operation
    {
        public Guid Id { get; set; }
        public string? Type { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public Guid GuardId { get; set; }
        public Guard Guard { get; set; }
        public Guid ReceiptId { get; set; }
        public Receipt Receipt { get; set; }
    }
}
