namespace Domain.Entities
{
    public class Guard
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public double Hours { get; set; }
        public Guid WorkerId { get; set; }
        public Worker Worker { get; set; }
        public List<Operation>? Operations { get; set; }
    }
}
