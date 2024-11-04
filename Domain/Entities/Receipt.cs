namespace Domain.Entities
{
    public class Receipt
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OperationId { get; set; }
        public Operation Operation { get; set; }
        public List<Inventory> Inventories { get; set; } = new List<Inventory>();
        public List<Instrument> Instruments { get; set; } = new List<Instrument>();
    }
}
