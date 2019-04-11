using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimetableGenerator.Models;
using TimetableGenerator.Services;

namespace TimetableGenerator.Controllers
{
    public class ClientController : Controller
    {
        private readonly TimetableConfigService _timetableConfigService;
        public ClientController(TimetableConfigService timetableConfigService)
        {
            _timetableConfigService = timetableConfigService;
        }

        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return View("TimetableConfig", _timetableConfigService.GetTimetableData(User.Identity.Name));
            }
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
