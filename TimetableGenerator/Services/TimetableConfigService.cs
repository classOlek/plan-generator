using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimetableGenerator.Models;
using TimetableGenerator.Models.DatabaseModels;
using TimetableGenerator.Models.LocalModels;
using TimetableGenerator.Models.SharedModels;

namespace TimetableGenerator.Services
{
    public class TimetableConfigService
    {
        private readonly DatabaseService _databaseService;

        public TimetableConfigService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public CreationStatus CreateCourseData(string data, string user, string sourceName)
        {
            try
            {
                IEnumerable<CourseData> courseList = JsonConvert.DeserializeObject<IEnumerable<CourseData>>(data);
                _databaseService.CreateCourseData(new TimetableDataDbModel
                {
                    Owner = user,
                    SourceName = sourceName,
                    Time = DateTime.UtcNow,
                    CourseList = courseList
                });
            }
            catch (Exception)
            {
                return CreationStatus.ExceptionThrown;
            }
            return CreationStatus.Created;
        }

        public IEnumerable<TimetableData> GetTimetableData(string name)
        {
            IEnumerable<TimetableDataDbModel> dbModelList = _databaseService.GetTimetableDataDbModels(name);
            if (dbModelList == null)
                return null;

            return GetTimetableData(dbModelList);
        }

        public TimetableData GetTimetableDataByHashCode(string name, int hashCode)
        {
            TimetableDataDbModel dbModel = _databaseService.GetTimetableDataByHashCode(name, hashCode);
            if(dbModel != null)
            {
                return new TimetableData
                {
                    Owner = dbModel.Owner,
                    SourceName = dbModel.SourceName,
                    Time = dbModel.Time,
                    HashCode = dbModel._id.GetHashCode(),
                    CourseList = dbModel.CourseList,
                    CourseLecturerSettings = dbModel.CourseLecturerSettings
                };
            }
            return null;
        }

        public bool UpdateUserCourseLecturerSettings(string name, int hashCode, IEnumerable<CourseLecturerSettings> courseLecturerSettings)
        {
            return _databaseService.UpdateUserCourseLecturerSettings(name, hashCode, courseLecturerSettings);
        }

        public IEnumerable<CourseLecturerSettings> GenerateCourseLecturerSettings(TimetableData timetableData)
        {
            List<CourseLecturerSettings> courseLecturerSettingsList = new List<CourseLecturerSettings>();
            foreach(CourseData data in timetableData.CourseList)
            {
                courseLecturerSettingsList.Add(new CourseLecturerSettings
                {
                    CourseName = data.CourseName,
                    CourseCode = data.CourseCode,
                    LecturerSettings = GenerateLecturerSettings(data)
                });
            }
            return courseLecturerSettingsList;
        }

        private IEnumerable<LecturerSettings> GenerateLecturerSettings(CourseData data)
        {
            List<string> lecturers = new List<string>();
            List<LecturerSettings> lecturerSettingsList = new List<LecturerSettings>();
            foreach(CourseDetails details in data.GroupList)
            {
                if (!lecturers.Contains(details.Lecturer))
                {
                    lecturers.Add(details.Lecturer);
                    lecturerSettingsList.Add(new LecturerSettings
                    {
                        Lecturer = details.Lecturer,
                        IsBlocked = false
                    });
                }
            }
            return lecturerSettingsList;
        }

        private IEnumerable<TimetableData> GetTimetableData(IEnumerable<TimetableDataDbModel> timetableDataDbModel)
        {
            return timetableDataDbModel.Select(dbModel => new TimetableData
            {
                Owner = dbModel.Owner,
                SourceName = dbModel.SourceName,
                Time = dbModel.Time,
                HashCode = dbModel._id.GetHashCode(),
                CourseList = dbModel.CourseList
            });
        }
    }
}
