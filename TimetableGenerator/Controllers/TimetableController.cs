using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimetableGenerator.Services;

namespace TimetableGenerator.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TimetableController : ControllerBase
    {
        private readonly TimetableConfigService _timetableConfigService;

        public TimetableController(TimetableConfigService timetableConfigService)
        {
            _timetableConfigService = timetableConfigService;
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
    }
}