namespace Application.DTOs
{
    public class ReceiptDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OperationId { get; set; }
        public List<InventoryDto>? Inventories { get; set; }
        public List<InstrumentDto>? Instruments { get; set; }
    }
}
