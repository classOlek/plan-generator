﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimetableGenerator.Models;

namespace TimetableGenerator.Controllers
{
    public class HomeController : Controller
    {

        private DatabaseHandler databaseHandler = DatabaseHandler.GetInstance();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MongoTest()
        {
            List<int> numberList = databaseHandler.GetTestNumbers();
            return View("MongoTest", numberList);
        }

        public IActionResult AddRandomNumber()
        {
            databaseHandler.AddTestNumber(new Random().Next(0, 100));
            return MongoTest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}