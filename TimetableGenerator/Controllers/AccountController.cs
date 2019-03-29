using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimetableGenerator.Models.LocalModels;
using TimetableGenerator.Services;

namespace TimetableGenerator.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        AccountService accountService = AccountService.GetInstance();

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] string login, [FromForm] string password)
        {
            if(IsUserLoggedIn(out User user))
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new { response = "User already logged in" });
            }
            if(accountService.GetUser(login, password) != null) {
                var claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, login),
                }, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await Request.HttpContext.SignInAsync("Cookies", claimsPrincipal);
                return Ok(new { response = "Logged in" });
            }

            return StatusCode((int)HttpStatusCode.NotFound, new { response = "Wrong username or password" });
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromForm] string login, [FromForm] string password, [FromForm] string password2)
        {
            if(!(CheckLoginFormat(login) && CheckPasswordFormat(password))) {
                return StatusCode((int)HttpStatusCode.BadRequest, 
                    new { response = "Incorrect login or password.\n" +
                    "Both need to be at least 6 characters long and contains only letters and digits." });
            }
            if(password != password2)
            {
                return StatusCode((int)HttpStatusCode.BadRequest,
                    new { response = "Password are not equal" });
            }

            UserCreationStatus status = accountService.CreateUser(login, password);
            switch(status)
            {
                case UserCreationStatus.UserExists:
                    return StatusCode((int)HttpStatusCode.Forbidden, new { response = "User already exists" });
                case UserCreationStatus.ExceptionThrown:
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { response = "Server error, please contact administrator" }); ;
                case UserCreationStatus.Created:
                default:
                    return Ok(new { response = "User created"});
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await Request.HttpContext.SignOutAsync();
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult IsLoggedIn()
        {
            return Ok(new { login = IsUserLoggedIn(out User user) });
        }

        [HttpGet]
        public IActionResult TestLogin()
        {
            return Ok(new { response = $"Logged in as {GetUserName()}"});
        }

        private bool IsUserLoggedIn(out User user)
        {
            user = accountService.GetUser(GetUserName());
            return user != null;
        }

        private string GetUserName()
        {
            try
            {
                return Request.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).Single();
            }
            catch(Exception)
            {
                return null;
            }
        }

        private bool CheckLoginFormat(string login)
        {
            if (login != null && login.All(Char.IsLetterOrDigit) && login.Length >= 6)
            {
                return true;
            }
            return false;
        }

        private bool CheckPasswordFormat(string password)
        {
            if(password != null && password.All(Char.IsLetterOrDigit) && password.Length >= 6)
            {
                return true;
            }
            return false;
        }
    }
}