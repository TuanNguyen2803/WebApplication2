using MongoDB.Driver;
using WebApplication2.AppDataContext;
using WebApplication2.Configuration;
using WebApplication2.DTO;
using WebApplication2.Models;

namespace WebApplication2.Service
{
    public class NumberInitializationService
    {
        private readonly AppDbContext _dbContext;
        private readonly ISystemJobSettings _jobSettings;
        public NumberInitializationService(AppDbContext dbContext, ISystemJobSettings systemJobSettings)
        {
            _dbContext = dbContext;
            _jobSettings = systemJobSettings;
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
        public void UpdateNumbersInChunks()
        {
            var filter = Builders<Number>.Filter.Empty;
            var sort = Builders<Number>.Sort.Ascending(x => x.ThoiGianCapNhat);
            int chunkSize=_jobSettings.chunkSize;

            var totalCount = _dbContext.Numbers.CountDocuments(filter);
            long processedCount = 0;

            while (processedCount < totalCount)
            {
                var chunk = _dbContext.Numbers.Find(filter)
                    .Sort(sort)
                    .Skip((int)processedCount)
                    .Limit(chunkSize)
                    .ToList();

                if (chunk.Count == 0)
                {
                    break;
                }

                foreach (var number in chunk)
                {
                    // Perform your update logic here. For example, change the value
                    var update = Builders<Number>.Update.Set(x => x.Value, new Random().Next(100, 200));
                    _dbContext.Numbers.UpdateOne(Builders<Number>.Filter.Eq(x => x.Id, number.Id), update);
                }

                processedCount += chunk.Count;
            }
        }
    }
}
