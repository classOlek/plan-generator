using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimetableGenerator.Models;
using TimetableGenerator.Models.LocalModels;
using TimetableGenerator.Models.SharedModels;

namespace TimetableGenerator.Services
{
    public class TimetableGeneratorService
    {
        private readonly int ResultLimit = 500;

        public IEnumerable<Timetable> GenerateTimetableList(TimetableData data, string conditions)
        {
            List<Timetable> returnList = new List<Timetable>();
            Rec(data.CourseList.ToList(), new List<CourseDetails>(), ref returnList);
            return returnList;
        }

        private void Rec(List<CourseData> courseList, List<CourseDetails> prevTimetable, ref List<Timetable> returnList)
        {
            if(courseList.Count() > 0)
            {
                CourseData courseData = courseList[courseList.Count() - 1];
                courseList.RemoveAt(courseList.Count() - 1);
                foreach(CourseDetails group in courseData.GroupList)
                {
                    if (returnList.Count() < ResultLimit)
                    {
                        List<CourseDetails> currentTimetable = prevTimetable.ToList();
                        currentTimetable.Add(group);
                        Rec(courseList.ToList(), currentTimetable, ref returnList);
                    }
                }
            } else
            {
                returnList.Add(new Timetable { GroupList = prevTimetable });
            }
        }
    }
}
