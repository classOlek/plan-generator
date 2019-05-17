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
        private MongoClient client;

        public DatabaseService()
        {
            MongoClientSettings settings = new MongoClientSettings
            {
                Server = new MongoServerAddress(Configuration.DatabaseIpAddress, Configuration.DatabasePort),
                UseSsl = false
            };
            client = new MongoClient(settings);
        }

        public bool UpdateUserConditions(string name, string conditions)
        {
            var updateDef = Builders<UserDbModel>.Update.Set(o => o.Conditions, conditions);
            try
            {
                GetUsersCollection().UpdateOne<UserDbModel>(o => o.Name == name, updateDef);
                return true;
            }catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TimetableDataDbModel> GetTimetableDataDbModels(string name)
        {
            try
            {
                return GetTimetableDataCollection()
                    .Find<TimetableDataDbModel>(m => m.Owner.Equals(name)).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TimetableDataDbModel GetTimetableDataByHashCode(string name, int hashCode)
        {
            IEnumerable<TimetableDataDbModel> models = GetTimetableDataDbModels(name);
            if(models != null)
                foreach (TimetableDataDbModel model in models)
                {
                    if (model._id.GetHashCode() == hashCode) return model;
                }
            return null;
        }

        public CreationStatus CreateCourseData(TimetableDataDbModel model)
        {
            try
            {
                GetTimetableDataCollection()
                    .InsertOne(model);
                return CreationStatus.Created;
            }
            catch (Exception)
            {
                return CreationStatus.ExceptionThrown;
            }
        }

        public CreationStatus CreateUser(string name, string hashedPassword)
        {
            try
            {
                GetUsersCollection()
                    .InsertOne(new UserDbModel { Name = name, HashedPassword = hashedPassword });
                return CreationStatus.Created;
            }
            catch (Exception)
            {
                return CreationStatus.ExceptionThrown;
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

        private IMongoDatabase GetDatabase()
        {
            return client.GetDatabase(Configuration.DatabaseName);
        }

        private IMongoCollection<UserDbModel> GetUsersCollection()
        {
            return GetDatabase().GetCollection<UserDbModel>(Configuration.DatabaseUsersCollectionName);
        }

        private IMongoCollection<TimetableDataDbModel> GetTimetableDataCollection()
        {
            return GetDatabase().GetCollection<TimetableDataDbModel>(Configuration.DatabaseTimetableDataCollectionName);
        }
    }
}
