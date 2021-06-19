using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.UI.Exceptions;
using SocialMedia.UI.Extensions;
using SocialMedia.UI.Models;
using SocialMedia.UI.QueryFilters;
using SocialMedia.UI.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialMedia.UI.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        readonly IUserService _userService;
        readonly ICountryService _countryService;
        string token = string.Empty;
        public UsersController(IUserService userService, ICountryService countryService)
        {
            _userService = userService;
            _countryService = countryService;
        }

        public IActionResult GetUsers()
        {
            return View();
        }

        [Produces("application/json")]
        public async Task<JsonResult> GetUsersJson([FromQuery] UserQueryFilter filter)
        {
            try
            {
                token = HttpContext.Session.GetString("JwtToken");
                var data = await _userService.GetUsers("SMUser", filter, token);

                return Json(new { data = data, result = "Ok" });
            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message, result = "Bad" });
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> NewUser()
        {
            var countries = await _countryService.GetCountries("Country");
            UserVM user = new UserVM { Countries = countries };
            return View(user);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> NewUser([FromForm] UserVM new_user)
        {
            try
            {
                await _userService.ManageUser("SMUser", new_user.SMUser, new_user.Image, 0, token);
                return RedirectToAction("Login", "Login");

            }
            catch (CustomApiException ex)
            {
                TempData["Message"] = ex.Errors.MyToString();
                return RedirectToAction("NewUser");
            }
            catch (Exception)
            {
                TempData["Message"] = "Sorry some error occured.";
                return View(new_user);

            }


        }

        public async Task<IActionResult> UpdateUser()
        {
            var countries = await _countryService.GetCountries("Country");
            var claim = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                token = HttpContext.Session.GetString("JwtToken");
                string id = claim.Value;
                var user_model = await _userService.GetUserById("SMUser", id, token);
                
                UserVM user_to_update = new UserVM { SMUser = user_model, Countries = countries };
                
                return View(user_to_update);
            }
            else
            {
                throw new Exception(); //Cambiar a CustomApiExceprion.
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromForm] UserVM user_to_update)
        {
            try
            {
                var claim = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    token = HttpContext.Session.GetString("JwtToken");
                    string id = claim.Value;
                    await _userService.ManageUser("SMUser", user_to_update.SMUser, user_to_update.Image, 1, token);
                    return RedirectToAction("GetUsers", "Users");
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (CustomApiException ex)
            {
                TempData["Message"] = ex.Errors.MyToString();
                return RedirectToAction("UpdateUser");
            }
            catch (Exception)
            {
                TempData["Message"] = "Sorry some error occured.";
                return RedirectToAction("UpdateUser");
            }



        }
        
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                token = HttpContext.Session.GetString("JwtToken");
                await _userService.DeleteUser("SMUser", id, token);

                return RedirectToAction("Login", "Logout");
            }
            catch (CustomApiException ex)
            {
                TempData["Message"] = ex.Errors.MyToString();
                return RedirectToAction("GetUsers");
            }
            catch (Exception)
            {
                TempData["Message"] = "Sorry some error occured.";
                return RedirectToAction("GetUsers");
            }
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordModel changePassword)
        {
            try
            {
                token = HttpContext.Session.GetString("JwtToken");
                await _userService.ChangePassword("AccountManagement/ChangePassword", changePassword, token);
                return RedirectToActionPermanent("Logout","Login");
            }
            catch (CustomApiException ex)
            {
                TempData["Message"] = ex.Errors.MyToString();
                return View();
            }
            catch (Exception)
            {
                TempData["Message"] = "Sorry some error occured.";
                return View();
            }
        }

        [AllowAnonymous]
        public IActionResult PasswordForgot()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PasswordForgot([FromForm] ForgotPasswordModel forgotPassword)
        {
            try
            {
                await _userService.ForgotPassword("AccountManagement/ResetPassword", forgotPassword);
                TempData["Message"] = "Please check your email to continue with the process.";
                return View();
            }
            catch (CustomApiException ex)
            {
                TempData["Message"] = ex.Errors.MyToString();
                return View();
            }
            catch (Exception)
            {
                TempData["Message"] = "Sorry some error occured.";
                return View();
            }
        }
       
    }
}
