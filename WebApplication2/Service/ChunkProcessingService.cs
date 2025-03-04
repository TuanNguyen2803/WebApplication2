using Hangfire;
using MongoDB.Driver;
using WebApplication2.AppDataContext;
using WebApplication2.Models;

namespace WebApplication2.Service
{
    public class ChunkProcessingService
    {
        private readonly AppDbContext _dbContext;

        public ChunkProcessingService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void ProcessChunk(int skip, int take)
        {
            var numbers = _dbContext.Numbers
                .Find(Builders<Number>.Filter.Empty)
                .Skip(skip)
                .Limit(take)
                .ToList();

            foreach (var number in numbers)
            {
                number.Value = new Random().Next(1, 100);
                number.ThoiGianCapNhat = DateTime.UtcNow;
                _dbContext.Numbers.ReplaceOne(x => x.Id == number.Id, number);
            }
        }

        public void ProcessAllInChunks(int chunkSize)
        {
            var totalCount = _dbContext.Numbers.CountDocuments(Builders<Number>.Filter.Empty);

            for (int skip = 0; skip < totalCount; skip += chunkSize)
            {
                var currentSkip = skip;
                BackgroundJob.Enqueue(() => ProcessChunk(currentSkip, chunkSize));
            }
        }
    }
}
