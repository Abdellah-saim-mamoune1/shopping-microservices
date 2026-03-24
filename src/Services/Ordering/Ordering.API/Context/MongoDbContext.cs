using MongoDB.Driver;
using Ordering.API.Entities;

namespace Ordering.API.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDb:ConnectionString"]);
            _database = client.GetDatabase(config["MongoDb:Database"]);
        }

        public IMongoCollection<Order> Orders =>
            _database.GetCollection<Order>("Orders");
    }

    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }=string.Empty;
        public string Database { get; set; }=string.Empty;
    }
}
