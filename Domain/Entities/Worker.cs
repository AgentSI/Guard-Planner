namespace Domain.Entities
{
    public class Worker
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? FirstName { get; set; }
        public string? Specialization { get; set; }
        public string Email { get; set; }
        public bool Available { get; set; }
        public int NoDaysVacation { get; set; } = 35;
        public bool IsGuard { get; set; }
        public double Percent { get; set; }
        public List<Guard>? Guards { get; set; }
    }
}
