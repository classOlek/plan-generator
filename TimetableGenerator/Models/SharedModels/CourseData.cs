using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimetableGenerator.Models.SharedModels
{
    public class CourseData
    {
        [JsonProperty("kod_kursu")]
        public string CourseCode { get; set; }

        [JsonProperty("nazwa_kursu")]
        public string CourseName { get; set; }

        [JsonProperty("dane")]
        public IEnumerable<CourseDetails> GroupList { get; set; }
    }

    public class CourseDetails
    {
        public string CourseName { get; set; }

        [JsonProperty("kod_grupy")]
        public string GroupCode { get; set; }

        [JsonProperty("stan")]
        public string Status { get; set; }

        [JsonProperty("prowadzacy")]
        public string Lecturer { get; set; }

        [JsonProperty("typ")]
        public string Type { get; set; }

        [JsonProperty("godzina")]
        public string Time { get; set; }
    }
}
