using System.Net;
using BabyMonitorApiBusiness.Abstractions;
using BabyMonitorApiDataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using BabyMonitorApiDataAccess.CustomValidationExceptions;
using BabyMonitorApiDataAccess.Dtos.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BabyMonitorApiDataAccess.Dtos.Users;

namespace BabyMonitorApi.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return Json("Hello");
        }

        [HttpPost("create")]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var createdUser = await _userService.PostUserAsync(user);
                    return Ok(createdUser);
                }
            }
            catch (EmailAlreadyInUseException ex)
            {
                ModelState.AddModelError("Email", ex.Message);
            }
            catch (UsernameAlreadyInUseException ex)
            {
                ModelState.AddModelError("Username", ex.Message);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> PostAuthenticateUser([FromBody] AuthenticateUserDto userData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string? email = userData.Email;
                    string? password = userData.Password;
                    User? user = await _userService.AuthenticateUserAsync(email, password);
                    Console.WriteLine(user);

                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.Name))
                        {
                            // Create authentication cookie
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.Email),
                                new Claim(ClaimTypes.Role, "Connected User"),
                            };

                            var claimsIdentity = new ClaimsIdentity(claims,
                                CookieAuthenticationDefaults.AuthenticationScheme);

                            var authProperties = new AuthenticationProperties
                            {
                                AllowRefresh = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120),
                                IsPersistent = true,
                                RedirectUri = "/Home"
                                // The full path or absolute URI to be used as an http 
                                // redirect response value.
                            };

                            // Sign in user
                            await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity),
                                authProperties);

                            return Ok(user);
                        }
                    }
                }
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Email", ex.Message);
            }
            catch (IncorrectPasswordException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Password", ex.Message);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task LogOutUser()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet("you")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                if (User.Identity != null && User.Identity.IsAuthenticated) 
                {
                    GetUserDto? user = await _userService.GetUserAsync(User.Identity.Name);
                    if (user != null)
                    {
                        return Ok(user);
                    }
                }
            }
            catch (UserNotFoundException ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                return NotFound(ModelState);
                throw;
            }
            return BadRequest();
        }

        [HttpPost("authenticate/server")]
        public async Task<IActionResult> PostAuthenticateUseOnServer([FromBody] AuthenticateUserDto userData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string? email = userData.Email;
                    string? password = userData.Password;
                    User? user = await _userService.AuthenticateUserCompareHashesAsync(email, password);

                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.Name))
                        {
                            return Ok(user);
                        }
                    }
                }
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Email", ex.Message);
            }
            catch (IncorrectPasswordException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Password", ex.Message);
            }

            return BadRequest(ModelState);
        }
    }
}
