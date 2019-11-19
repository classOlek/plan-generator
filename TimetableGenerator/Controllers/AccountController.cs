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
        public async Task<IActionResult> Login([FromForm] string login, [FromForm] string password, [FromForm] int shouldRedirect)
        {
            if(IsUserLoggedIn(out User user))
            {
                if (shouldRedirect == 1)
                {
                    return RedirectToAction("Index", "Client", new { msg = "User already logged in!" });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.Forbidden, new { response = "User already logged in!" });
                }
            }
            if(_accountService.GetUser(login, password) != null) {
                var claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, login),
                }, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await Request.HttpContext.SignInAsync("Cookies", claimsPrincipal);
                if(shouldRedirect == 1)
                {
                    return RedirectToAction("Index", "Client", new { msg = "User logged in!" });
                } else
                {
                    return Ok(new { response = "User logged in" });
                }
            }
            if (shouldRedirect == 1)
            {
                return RedirectToAction("Index", "Client", new { msg = "Wrong username or password!" });
            }
            else
            {
                return StatusCode((int)HttpStatusCode.NotFound, new { response = "Wrong username or password" });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromForm] string login, [FromForm] string password, [FromForm] string password2, [FromForm] int shouldRedirect)
        {
            if(!(CheckLoginFormat(login) && CheckPasswordFormat(password))) {
                if (shouldRedirect == 1)
                {
                    return RedirectToAction("Index", "Client", new { msg = "Incorrect login or password.\n" +
                    "Both need to be at least 6 characters long and contains only letters and digits."
                    });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest,
                    new
                    {
                        response = "Incorrect login or password.\n" +
                    "Both need to be at least 6 characters long and contains only letters and digits."
                    });
                }
            }
            if(password != password2)
            {
                if (shouldRedirect == 1)
                {
                    return RedirectToAction("Index", "Client", new { msg = "Password are not equal!" });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest,
                        new { response = "Password are not equal!" });
                }
            }

            CreationStatus status = _accountService.CreateUser(login, password);
            switch(status)
            {
                case CreationStatus.AlreadyExists:
                    if (shouldRedirect == 1)
                    {
                        return RedirectToAction("Index", "Client", new { msg = "User already exists!" });
                    }
                    else
                    {
                        return StatusCode((int)HttpStatusCode.Forbidden, new { response = "User already exists" });
                    }
                case CreationStatus.ExceptionThrown:
                    if (shouldRedirect == 1)
                    {
                        return RedirectToAction("Index", "Client", new { msg = "Server error, please contact administrator!" });
                    }
                    else
                    {
                        return StatusCode((int)HttpStatusCode.InternalServerError, new { response = "Server error, please contact administrator" });
                    }
                case CreationStatus.Created:
                default:
                    if (shouldRedirect == 1)
                    {
                        return RedirectToAction("Index", "Client", new { msg = "User created!" });
                    }
                    else
                    {
                        return Ok(new { response = "User created" });
                    }
            }
        }

        [HttpPost("updateConditions")]
        public IActionResult UpdateConditions([FromForm] string saveFormData, [FromForm] int shouldRedirect)
        {
            if (IsValidJson(saveFormData) && _accountService.UpdateUserConditions(GetUserName(), saveFormData)){
                if (shouldRedirect == 1)
                {
                    return RedirectToAction("Index", "Client", new { msg = "Conditions updated!" });
                }
                else
                {
                    return Ok(new { response = "Conditions saved" });
                }
            }
            if (shouldRedirect == 1)
            {
                return RedirectToAction("Index", "Client", new { msg = "Server error, please contact administrator!" });
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { response = "Server error, please contact administrator" });
            }
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout(int shouldRedirect)
        {
            await Request.HttpContext.SignOutAsync();
            if (shouldRedirect == 1)
            {
                return RedirectToAction("Index", "Client", new { msg = "User logged out!" });
            }
            else
            {
                return Ok(new { response = "User logged out" });
            }
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
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) ||
                (strInput.StartsWith("[") && strInput.EndsWith("]")))
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex)
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