using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimetableGenerator.Models
{
    public class ValueEntity
    {
        public ObjectId _id { get; set; }
        public int value { get; set; }
    }
}
