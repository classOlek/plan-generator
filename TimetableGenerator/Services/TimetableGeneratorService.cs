using Newtonsoft.Json.Linq;
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
            List<CourseTime> courseTime = null;
            if (conditions != "")
            {
                courseTime = ParseConditionsToCourseTime(conditions);
            } else
            {
                courseTime = new List<CourseTime>();
            }
            Rec(SetupCourseData(data), new List<CourseDetails>(), courseTime, ref returnList);
            return returnList;
        }

        private void Rec(List<CourseData> courseList, List<CourseDetails> prevTimetable, List<CourseTime> usedTime, ref List<Timetable> returnList)
        {
            if(courseList.Count() > 0)
            {
                CourseData courseData = courseList[courseList.Count() - 1];
                courseList.RemoveAt(courseList.Count() - 1);
                foreach(CourseDetails group in courseData.GroupList)
                {
                    string[] splittedStatus = group.Status.Split("/");
                    int currentStudents = int.Parse(splittedStatus[0]);
                    int maxStudents = int.Parse(splittedStatus[1]);
                    if (returnList.Count() < ResultLimit)
                    {
                        CourseTime courseTime = GetCourseTime(group);
                        if (!IsCourseTimeColliding(courseTime, usedTime) && currentStudents < maxStudents)
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

        private List<CourseData> SetupCourseData(TimetableData data)
        {
            List<CourseData> courseDataList = data.CourseList.ToList();
            for(int i = 0; i < courseDataList.Count; i++)
            {
                if(ShouldDeleseCourseFromList(courseDataList[i].CourseCode, data.CourseLecturerSettings))
                {
                    courseDataList.RemoveAt(i--);
                }
                else
                {
                    List<CourseDetails> details = courseDataList[i].GroupList.ToList();
                    for (int j = 0; j < details.Count; j++)
                    {
                        if(ShouldRemoveLecturerFromCourse(courseDataList[i].CourseCode, details[j].Lecturer, data.CourseLecturerSettings))
                        {
                            details.RemoveAt(j--);
                        }
                    }
                    courseDataList[i].GroupList = details;
                }
            }
            return courseDataList;
        }

        private bool ShouldDeleseCourseFromList(string courseCode, IEnumerable<CourseLecturerSettings> settings)
        {
            foreach(CourseLecturerSettings s in settings)
            {
                if(s.CourseCode == courseCode)
                {
                    foreach(LecturerSettings ls in s.LecturerSettings)
                    {
                        if (ls.IsBlocked == false) return false;
                    }
                    return true;
                }
            }
            return false;
        }

        private bool ShouldRemoveLecturerFromCourse(string courseCode, string lecturer, IEnumerable<CourseLecturerSettings> settings)
        {
            foreach (CourseLecturerSettings s in settings)
            {
                if (s.CourseCode == courseCode)
                {
                    foreach (LecturerSettings ls in s.LecturerSettings)
                    {
                        if (ls.Lecturer == lecturer)
                        {
                            if(ls.IsBlocked == true)
                                return true;
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        private List<CourseTime> ParseConditionsToCourseTime(string conditions)
        {
            List<CourseTime> courseTimeList = new List<CourseTime>();
            JArray jsonArray = JArray.Parse(conditions);
            foreach(dynamic condition in jsonArray)
            {
                string type = condition.type;

                string time = condition.hour;
                string day = condition.day;
                bool everyday = false;
                switch (day)
                {
                    case "Monday":
                        day = "pn";
                        break;
                    case "Tuesday":
                        day = "wt";
                        break;
                    case "Wednesday":
                        day = "śr";
                        break;
                    case "Thursday":
                        day = "cz";
                        break;
                    case "Friday":
                        day = "pt";
                        break;
                    case "Everyday":
                        everyday = true;
                        break;
                }

                switch (type)
                {
                    case "noClassesBefore":
                        if (everyday)
                        {
                            courseTimeList.Add(CreateCourseTime("pn", "7:00", time));
                            courseTimeList.Add(CreateCourseTime("wt", "7:00", time));
                            courseTimeList.Add(CreateCourseTime("śr", "7:00", time));
                            courseTimeList.Add(CreateCourseTime("cz", "7:00", time));
                            courseTimeList.Add(CreateCourseTime("pt", "7:00", time));
                        }
                        else
                        {
                            courseTimeList.Add(CreateCourseTime(day, "7:00", time));
                        }
                        break;
                    case "noClassesAfter":
                      
                        if(everyday)
                        {
                            courseTimeList.Add(CreateCourseTime("pn", time, "23:00"));
                            courseTimeList.Add(CreateCourseTime("wt", time, "23:00"));
                            courseTimeList.Add(CreateCourseTime("śr", time, "23:00"));
                            courseTimeList.Add(CreateCourseTime("cz", time, "23:00"));
                            courseTimeList.Add(CreateCourseTime("pt", time, "23:00"));
                        } else
                        {
                            courseTimeList.Add(CreateCourseTime(day, time, "23:00"));
                        }
                        break;
                }
            }
            return courseTimeList;
        }

        private CourseTime CreateCourseTime(string day, string courseStart, string courseEnd)
        {
            return new CourseTime
            {
                EvenWeek = true,
                OddWeek = true,
                Day = day,
                CourseStart = DateTime.Parse(courseStart),
                CourseEnd = DateTime.Parse(courseEnd)
            };
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
