namespace Application.DTOs
{
    public class DailyWorkHoursDto
    {
        public Guid Id { get; set; }
        public Guid WorkerHoursId { get; set; }
        public int Day { get; set; }
        public int HoursWorked { get; set; }
    }
}
