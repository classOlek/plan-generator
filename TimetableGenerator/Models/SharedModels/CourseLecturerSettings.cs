using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimetableGenerator.Models.SharedModels
{
    public class CourseLecturerSettings
    {
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public IEnumerable<LecturerSettings> LecturerSettings { get; set; }

    }
}
