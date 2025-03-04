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
    }
}
