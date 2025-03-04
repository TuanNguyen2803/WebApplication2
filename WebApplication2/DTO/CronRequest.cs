namespace WebApplication2.DTO
{
    public class CronRequest
    {
        public int? Minute { get; set; }
        public int? Hour { get; set; }
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? DayOfWeek { get; set; }
        public int? Year { get; set; }
    }
}
