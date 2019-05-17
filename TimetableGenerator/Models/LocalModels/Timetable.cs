using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimetableGenerator.Models.SharedModels;

namespace TimetableGenerator.Models.LocalModels
{
    public class Timetable
    {
        public IEnumerable<CourseDetails> GroupList { get; set; }
    }
}
