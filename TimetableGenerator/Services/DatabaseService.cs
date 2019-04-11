using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimetableGenerator.Models;
using TimetableGenerator.Models.DatabaseModels;
using TimetableGenerator.Models.LocalModels;

namespace TimetableGenerator
{
    public class DatabaseService
    {
        private static DatabaseService instance = null;
        private MongoClient client;

        public static DatabaseService GetInstance()
        {
            if(instance == null)
            {
                instance = new DatabaseService();
            }
            return instance;
        }

        public UserCreationStatus CreateUser(string name, string hashedPassword)
        {
            try
            {
                GetUsersCollection()
                    .InsertOne(new UserDbModel { Name = name, HashedPassword = hashedPassword });
                return UserCreationStatus.Created;
            }
            catch (Exception)
            {
                return UserCreationStatus.ExceptionThrown;
            }
        }

        public UserDbModel GetUser(string name)
        {
            try
            {
                return GetUsersCollection()
                    .Find<UserDbModel>(m => m.Name.Equals(name))
                    .FirstOrDefault();
            } catch (Exception)
            {
                return null;
            }
        }

        private DatabaseService()
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

        private IMongoCollection<UserDbModel> GetUsersCollection()
        {
            return GetDatabase().GetCollection<UserDbModel>(Configuration.DatabaseUsersCollectionName);
        }
    }
}
