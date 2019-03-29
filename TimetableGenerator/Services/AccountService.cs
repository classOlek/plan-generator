using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimetableGenerator.Models.DatabaseModels;
using TimetableGenerator.Models.LocalModels;

namespace TimetableGenerator.Services
{
    public class AccountService
    {
        private static AccountService instance;

        private DatabaseService databaseService;

        public static AccountService GetInstance()
        {
            if(instance == null)
            {
                instance = new AccountService();
            }
            return instance;
        }        

        private AccountService()
        {
            databaseService = DatabaseService.GetInstance();
        }

        public UserCreationStatus CreateUser(string name, string password)
        {
            if(GetUser(name) != null)
            {
                return UserCreationStatus.UserExists;
            }
            return databaseService.CreateUser(name, CryptographyService.GetInstance().sha256(password));
        }

        public User GetUser(string name)
        {
            UserDbModel userDbModel = databaseService.GetUser(name);
            if (userDbModel == null)
                return null;

            return GetUser(userDbModel);
        }

        public User GetUser(string name, string password)
        {
            UserDbModel userDbModel = databaseService.GetUser(name);
            if(userDbModel == null || CryptographyService.GetInstance().sha256(password) != userDbModel.HashedPassword)
            {
                return null;
            }
            return GetUser(userDbModel);
        }

        private User GetUser(UserDbModel userDbModel)
        {
            return new User
            {
                Name = userDbModel.Name
            };
        }
    }
}
