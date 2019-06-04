using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimetableGenerator.Models.SharedModels;

namespace TimetableGenerator.Models
{
    public class TimetableData
    {
        public string Owner { get; set; }
        public string SourceName { get; set; }
        public DateTime Time { get; set; }
        public int HashCode { get; set; }
        public IEnumerable<CourseData> CourseList { get; set; }
        public IEnumerable<CourseLecturerSettings> CourseLecturerSettings { get; set; }
    }
}
