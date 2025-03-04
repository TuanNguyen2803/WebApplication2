using MongoDB.Driver;
using WebApplication2.AppDataContext;
using WebApplication2.DTO;
using WebApplication2.Models;

namespace WebApplication2.Service
{
    public class NumberInitializationService
    {
        private readonly AppDbContext _dbContext;

        public NumberInitializationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InitializeNumbers()
        {
            _dbContext.Numbers.DeleteMany(Builders<Number>.Filter.Empty);

            for (int i = 0; i < 5; i++)
            {
                var number = new Number
                {
                    Value = new Random().Next(1, 100),
                    ThoiGianCapNhat = DateTime.UtcNow
                };
                _dbContext.Numbers.InsertOne(number);
            }
        }
        public string ConvertToCron(CronRequest request)
        {
            string minute = request.Minute.HasValue ? request.Minute.Value.ToString() : "*";
            string hour = request.Hour.HasValue ? request.Hour.Value.ToString() : "*";
            string day = request.Day.HasValue ? request.Day.Value.ToString() : "*";
            string month = request.Month.HasValue ? request.Month.Value.ToString() : "*";
            string dayOfWeek = request.DayOfWeek.HasValue ? request.DayOfWeek.Value.ToString() : "?";
            string year = request.Year.HasValue ? request.Year.Value.ToString() : "*";

            return $"{minute} {hour} {day} {month} {dayOfWeek} {year}";
        }
    }
}
