using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimetableGenerator.Models;

namespace TimetableGenerator
{
    public class DatabaseHandler
    {
        private static DatabaseHandler instance = null;
        private MongoClient client;

        public static DatabaseHandler GetInstance()
        {
            if(instance == null)
            {
                instance = new DatabaseHandler();
            }
            return instance;
        }

        public void AddTestNumber(int number)
        {
            GetTestCollection().InsertOne(new ValueEntity { value = number });
        }

        public List<int> GetTestNumbers()
        {
            return GetTestCollection().Find(_ => true)
                .ToList<ValueEntity>()
                .Select(valueEntity => valueEntity.value)
                .ToList<int>();
        }

        private DatabaseHandler()
        {
            MongoClientSettings settings = new MongoClientSettings
            {
                Server = new MongoServerAddress(Configuration.DatabaseIpAddress, Configuration.DatabasePort),
                UseSsl = false
            };
            client = new MongoClient(settings);
        }


        private IMongoDatabase GetDatabase()
        {
            return client.GetDatabase(Configuration.DatabaseName);
        }

        private IMongoCollection<ValueEntity> GetTestCollection()
        {
            return GetDatabase().GetCollection<ValueEntity>("TestIntCollection");
        }
    }
}
