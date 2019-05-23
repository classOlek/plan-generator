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
        private readonly string LECTURE_TYPE = "Wykład";

        private readonly int ResultLimit = 500;
        private readonly bool IgnoreLectures = false;

        public IEnumerable<Timetable> GenerateTimetableList(TimetableData data, string conditions)
        {
            List<Timetable> returnList = new List<Timetable>();
            Rec(data.CourseList.ToList(), new List<CourseDetails>(), new List<CourseTime>(), ref returnList);
            return returnList;
        }

        private void Rec(List<CourseData> courseList, List<CourseDetails> prevTimetable, List<CourseTime> usedTime, ref List<Timetable> returnList)
        {
            if(courseList.Count() > 0)
            {
                CourseData courseData = courseList[courseList.Count() - 1];
                courseList.RemoveAt(courseList.Count() - 1);
                if(courseData.CourseName == "Zaaw. technologie webowe")
                {
                    string x = "x";
                }
                foreach(CourseDetails group in courseData.GroupList)
                {
                    if (returnList.Count() < ResultLimit)
                    {
                        CourseTime courseTime = GetCourseTime(group);
                        if (!IsCourseTimeColliding(courseTime, usedTime))
                        {
                            List<CourseTime> usedTimeCopy = usedTime.ToList();
                            if (courseTime != null && !(IgnoreLectures && group.Type == LECTURE_TYPE)) usedTimeCopy.Add(courseTime);
                            List<CourseDetails> currentTimetable = prevTimetable.ToList();
                            group.CourseName = courseData.CourseName;
                            currentTimetable.Add(group);
                            Rec(courseList.ToList(), currentTimetable, usedTimeCopy, ref returnList);
                        }
                    }
                }
            } else
            {
                if(prevTimetable.Count > 0) returnList.Add(new Timetable { GroupList = prevTimetable });
            }
        }

        private bool IsCourseTimeColliding(CourseTime courseTime, List<CourseTime> usedTime)
        {
            if (courseTime == null) return false;
            foreach(CourseTime used in usedTime)
            {
                if (courseTime.Day == used.Day && (courseTime.EvenWeek == used.EvenWeek || courseTime.OddWeek == used.OddWeek))
                {
                    if (courseTime.CourseStart >= used.CourseStart && courseTime.CourseStart <= used.CourseEnd ||
                        courseTime.CourseEnd >= used.CourseStart && courseTime.CourseEnd <= used.CourseEnd)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private CourseTime GetCourseTime(CourseDetails details)
        {
            try
            {
                string[] splittedTime = details.Time.Split(" ");
                string dayPart = splittedTime[0];
                bool oddWeek = true;
                bool evenWeek = true;
                if (dayPart.Contains("/"))
                {
                    if (dayPart.Contains("TP")) oddWeek = false;
                    if (dayPart.Contains("TN")) evenWeek = false;
                    dayPart = dayPart.Split("/")[0];
                }
                string[] splittedHour = splittedTime[1].Split("-");
                return new CourseTime
                {
                    Day = dayPart,
                    CourseStart = DateTime.Parse(splittedHour[0]),
                    CourseEnd = DateTime.Parse(splittedHour[1]),
                    EvenWeek = evenWeek,
                    OddWeek = oddWeek
                };
            } catch (Exception)
            {
                return null;
            }
        }
    }
}
