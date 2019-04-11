using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimetableGenerator
{
    public class Configuration
    {
        public static string DatabaseIpAddress = "localhost";
        public static int DatabasePort = 27017;
        public static string DatabaseName = "TimetableGenerator";
        public static string DatabaseUsersCollectionName = "Users";
        public static string DatabaseTimetableDataCollectionName = "TimetableData";
    }
}