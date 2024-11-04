namespace Domain.Entities
{
    public class Vacation
    {
        public Guid Id { get; set; }
        public Guid WorkerId { get; set; }
        public Worker Worker { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Reason { get; set; }
        public int NoDays { get; set; }
    }
}
