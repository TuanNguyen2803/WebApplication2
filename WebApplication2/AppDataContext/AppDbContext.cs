using MongoDB.Driver;
using WebApplication2.Models;

namespace WebApplication2.AppDataContext
{
    public class AppDbContext
    {
        private readonly IMongoDatabase _database;

        public AppDbContext(IMongoClient client, IConfiguration configuration)
        {
            _database = client.GetDatabase(new MongoUrl(configuration.GetConnectionString("MongoDb")).DatabaseName);
        }

        public IMongoCollection<Number> Numbers => _database.GetCollection<Number>("Numbers");
    }
}
