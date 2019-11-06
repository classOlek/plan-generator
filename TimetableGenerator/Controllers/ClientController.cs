using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimetableGenerator.Models;
using TimetableGenerator.Models.LocalModels;
using TimetableGenerator.Services;

namespace TimetableGenerator.Controllers
{
    public class ClientController : Controller
    {
        private readonly TimetableConfigService _timetableConfigService;
        private readonly AccountService _accountService;
        public ClientController(TimetableConfigService timetableConfigService, AccountService accountService)
        {
            _timetableConfigService = timetableConfigService;
            _accountService = accountService;
        }

        public IActionResult Index(string msg)
        {
            ViewBag.ClientMsg = msg;
            if (User.Identity.IsAuthenticated)
            {
                return View("TimetableConfig", new TimetableConfig
                {
                    User = _accountService.GetUser(GetUserName()),
                    TimetableDataList = _timetableConfigService.GetTimetableData(User.Identity.Name)
                });
            }
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetUserName()
        {
            try
            {
                return Request.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
