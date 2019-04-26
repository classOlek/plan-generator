using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TimetableGenerator.Models;
using TimetableGenerator.Models.LocalModels;
using TimetableGenerator.Services;

namespace TimetableGenerator.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {

        AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] string login, [FromForm] string password)
        {
            if(IsUserLoggedIn(out User user))
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new { response = "User already logged in" });
            }
            if(_accountService.GetUser(login, password) != null) {
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
        [HttpPost("register")]
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

            CreationStatus status = _accountService.CreateUser(login, password);
            switch(status)
            {
                case CreationStatus.AlreadyExists:
                    return StatusCode((int)HttpStatusCode.Forbidden, new { response = "User already exists" });
                case CreationStatus.ExceptionThrown:
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { response = "Server error, please contact administrator" });
                case CreationStatus.Created:
                default:
                    return Ok(new { response = "User created"});
            }
        }

        [HttpPost("updateConditions")]
        public IActionResult UpdateConditions([FromForm] string saveFormData)
        {
            if (IsValidJson(saveFormData) && _accountService.UpdateUserConditions(GetUserName(), saveFormData)){
                return Ok(new { response = "Conditions saved" });
            }
            return StatusCode((int)HttpStatusCode.InternalServerError, new { response = "Server error, please contact administrator" });
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await Request.HttpContext.SignOutAsync();
            return Ok(new { response = "User logged out" });
        }

        [AllowAnonymous]
        [HttpGet("isLoggedIn")]
        public IActionResult IsLoggedIn()
        {
            return Ok(new { login = IsUserLoggedIn(out User user) });
        }

        [HttpGet("testLogin")]
        public IActionResult TestLogin()
        {
            return Ok(new { response = $"Logged in as {GetUserName()}"});
        }

        private bool IsUserLoggedIn(out User user)
        {
            user = _accountService.GetUser(GetUserName());
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

        private bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}