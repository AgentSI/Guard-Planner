namespace Application.DTOs
{
    public class WorkerHoursDto
    {
        public Guid Id { get; set; }
        public Guid WorkerId { get; set; }
        public string? WorkerName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int HoursWorked { get; set; }
        public List<DailyWorkHoursDto>? DailyWorkHours { get; set; }
    }
}
