using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimetableGenerator.Models.LocalModels
{
    public class CourseTime
    {
        public DateTime CourseStart { get; set; }
        public DateTime CourseEnd { get; set; }
        public string Day { get; set; }
        public bool OddWeek { get; set; }
        public bool EvenWeek { get; set; }
    }
}
