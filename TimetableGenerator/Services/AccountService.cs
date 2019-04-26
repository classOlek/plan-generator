using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimetableGenerator.Models;
using TimetableGenerator.Models.DatabaseModels;
using TimetableGenerator.Models.LocalModels;

namespace TimetableGenerator.Services
{
    public class AccountService
    {
        private readonly DatabaseService _databaseService;
        private readonly CryptographyService _cryptographyService;

        public AccountService(DatabaseService databaseService, CryptographyService cryptographyService)
        {
            _databaseService = databaseService;
            _cryptographyService = cryptographyService;
        }

        public CreationStatus CreateUser(string name, string password)
        {
            if(GetUser(name) != null)
            {
                return CreationStatus.AlreadyExists;
            }
            return _databaseService.CreateUser(name, _cryptographyService.sha256(password));
        }

        public bool UpdateUserConditions(string name, string conditions)
        {
            return _databaseService.UpdateUserConditions(name, conditions);
        }

        public User GetUser(string name)
        {
            UserDbModel userDbModel = _databaseService.GetUser(name);
            if (userDbModel == null)
                return null;

            return GetUser(userDbModel);
        }

        public User GetUser(string name, string password)
        {
            UserDbModel userDbModel = _databaseService.GetUser(name);
            if(userDbModel == null || _cryptographyService.sha256(password) != userDbModel.HashedPassword)
            {
                return null;
            }
            return GetUser(userDbModel);
        }

        private User GetUser(UserDbModel userDbModel)
        {
            return new User
            {
                Name = userDbModel.Name,
                Conditions = userDbModel.Conditions
            };
        }
    }
}
