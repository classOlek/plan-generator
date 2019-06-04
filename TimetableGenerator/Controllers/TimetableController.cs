using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimetableGenerator.Models;
using TimetableGenerator.Models.LocalModels;
using TimetableGenerator.Models.SharedModels;
using TimetableGenerator.Services;

namespace TimetableGenerator.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TimetableController : ControllerBase
    {
        private readonly TimetableConfigService _timetableConfigService;
        private readonly TimetableGeneratorService _timetableGeneratorService;

        public TimetableController(TimetableConfigService timetableConfigService, TimetableGeneratorService timetableGeneratorService)
        {
            _timetableConfigService = timetableConfigService;
            _timetableGeneratorService = timetableGeneratorService;
        }

        [HttpPost("uploadCourses")]
        public IActionResult UploadCurses(IFormFile file)
        {
            if(file == null)
            {
                return StatusCode((int)HttpStatusCode.UnsupportedMediaType, new { response = "file null" });
            } else if (!file.FileName.EndsWith(".json"))
            {
                return StatusCode((int)HttpStatusCode.UnsupportedMediaType, new { response = ".json required" });
            }

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string result = reader.ReadToEnd();
                if (_timetableConfigService.CreateCourseData(result, User.Identity.Name, file.FileName) == Models.LocalModels.CreationStatus.Created) {
                    return Ok(new { response = "success" });
                }
            }
            return StatusCode((int)HttpStatusCode.InternalServerError, new { response = "Something went wrong!" });
        }

        [HttpGet("getCourseData")]
        public IActionResult GetCourseData([FromQuery] int hashCode)
        {
            TimetableData data = _timetableConfigService.GetTimetableDataByHashCode(User.Identity.Name, hashCode);
            if (data != null)
            {
                if (data.CourseLecturerSettings == null) data.CourseLecturerSettings = _timetableConfigService.GenerateCourseLecturerSettings(data);
                return Ok(new { response = "success", data = data.CourseLecturerSettings });
            }
            return Ok(new { response = "not found" });
        }

        [HttpPost("generateTimetable")]
        public IActionResult GenerateTimetable([FromQuery] int hashCode, [FromBody] IEnumerable<CourseLecturerSettings> courseLecturerSettings)
        {
            bool courseUpdate = _timetableConfigService.UpdateUserCourseLecturerSettings(User.Identity.Name, hashCode, courseLecturerSettings);

            TimetableData data = _timetableConfigService.GetTimetableDataByHashCode(User.Identity.Name, hashCode);
            if(!courseUpdate) return Ok(new { response = "failed to update course lecturer settings!" });
            if (data != null)
            {
                IEnumerable<Timetable> timetableList = _timetableGeneratorService.GenerateTimetableList(data, "");
                return Ok(new { response = "success", data = timetableList });
            }
            return Ok(new { response = "not found" });
        }
    }
}