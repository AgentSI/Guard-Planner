namespace Domain.Entities
{
    public class WorkerHours
    {
        public Guid Id { get; set; }
        public Guid WorkerId { get; set; }
        public Worker? Worker { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int HoursWorked { get; set; }
        public List<DailyWorkHours>? DailyWorkHours { get; set; }
    }
}
