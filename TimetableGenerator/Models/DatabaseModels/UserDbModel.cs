using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimetableGenerator.Models.DatabaseModels
{
    public class UserDbModel
    {
        public ObjectId _id { get; set; }
        public string Name { get; set; }
        public string HashedPassword { get; set; }
    }
}
