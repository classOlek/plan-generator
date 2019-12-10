using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TimetableGenerator.Models;
using TimetableGenerator.Models.LocalModels;
using TimetableGenerator.Models.SharedModels;
using TimetableGenerator.Services;
using Xunit;

namespace TimetableGeneratorTest
{
    public class TimetableGeneratorTest
    {
        [Fact]
        public void TimetableCountTest()
        {
            TimetableGeneratorService timetableGeneratorService = new TimetableGeneratorService();

            Assert.Empty(timetableGeneratorService.GenerateTimetableList(new TimetableData { CourseList = new List<CourseData>(), CourseLecturerSettings = GetCourseLecturerSettings() }, ""));
            Assert.Single(timetableGeneratorService.GenerateTimetableList(new TimetableData { CourseList = GetCourseDataMock(), CourseLecturerSettings = GetCourseLecturerSettings() }, ""));
            Assert.Single(timetableGeneratorService.GenerateTimetableList(new TimetableData { CourseList = GetCourseDataMock().Concat(GetCourseDataMock()), CourseLecturerSettings = GetCourseLecturerSettings() }, ""));
            Assert.Equal(2, timetableGeneratorService.GenerateTimetableList(new TimetableData { CourseList = GetCourseWithTwoGroupsDataMock(), CourseLecturerSettings = GetCourseLecturerSettings() }, "").Count());
            Assert.Equal(2, timetableGeneratorService.GenerateTimetableList(new TimetableData { CourseList = GetCourseWithTwoGroupsDataMock().Concat(GetCourseDataMock()), CourseLecturerSettings = GetCourseLecturerSettings() }, "").Count());
            Assert.Equal(4, timetableGeneratorService.GenerateTimetableList(new TimetableData { CourseList = GetCourseWithTwoGroupsDataMock().Concat(GetCourseWithTwoGroupsDataMock()), CourseLecturerSettings = GetCourseLecturerSettings() }, "").Count());
            Assert.Equal(8, timetableGeneratorService.GenerateTimetableList(new TimetableData { CourseList = GetCourseWithTwoGroupsDataMock().Concat(GetCourseWithTwoGroupsDataMock()).Concat(GetCourseWithTwoGroupsDataMock()), CourseLecturerSettings = GetCourseLecturerSettings() }, "").Count());
        }

        private IEnumerable<CourseData> GetCourseDataMock()
        {
            return new List<CourseData> { new CourseData {
                CourseCode = "mockCourseCode",
                CourseName = "mockCourseName",
                GroupList = new List<CourseDetails>
                {
                    new CourseDetails
                    {
                        Lecturer = "mockLecturer",
                        GroupCode = "mockGroupCode",
                        Status = "1/5",
                        Time = "mockTime",
                        Type = "mockType"
                    }
                }
            } };
        }

        private List<CourseLecturerSettings> GetCourseLecturerSettings()
        {
            return new List<CourseLecturerSettings>();
        }

        private IEnumerable<CourseData> GetCourseWithTwoGroupsDataMock()
        {
            return new List<CourseData> { new CourseData {
                CourseCode = "mockCourseCode",
                CourseName = "mockCourseName",
                GroupList = new List<CourseDetails>
                {
                    new CourseDetails
                    {
                        Lecturer = "mockLecturer",
                        GroupCode = "mockGroupCode",
                        Status = "1/5",
                        Time = "mockTime",
                        Type = "mockType"
                    },
                    new CourseDetails
                    {
                        Lecturer = "mockLecturer",
                        GroupCode = "mockGroupCode",
                        Status = "1/5",
                        Time = "mockTime",
                        Type = "mockType"
                    }
                }
            } };
        }
    }
}
