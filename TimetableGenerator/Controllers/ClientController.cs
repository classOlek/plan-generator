﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimetableGenerator.Models;

namespace TimetableGenerator.Controllers
{
    public class ClientController : Controller
    {

        private DatabaseService databaseHandler = DatabaseService.GetInstance();

        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public IActionResult Login([FromForm] string login, [FromForm] string password)
        {
            return Index();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}