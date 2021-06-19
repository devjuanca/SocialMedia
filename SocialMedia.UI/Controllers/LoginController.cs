using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.UI.Services;
using SocialMedia.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using SocialMedia.UI.Exceptions;
using SocialMedia.UI.Extensions;
namespace SocialMedia.UI.Controllers
{
    public class LoginController : Controller
    {
        ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel login)
        {
            try
            {
                ViewData["Message"] = string.Empty;

                var data_token = await _loginService.Login(login); //Hago el login y recupero objeto.
                var id = data_token.data.Id;
                var roles = data_token.data.Roles;

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme); // creo in claimsIdentity con el tipo de autenticacion usada.
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, id)); // añado al claim el Id

                if (roles != null)
                    foreach (var item in roles)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, item));  //añado al claim los roles.
                    }

                var principal = new ClaimsPrincipal(identity); // creo un Claim principal para signIn con el claim creado anteriormente.

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString("JwtToken", data_token.data.Token); //Guardo el Token en Sesion.

                return RedirectToAction("GetUsers", "Users");
            }
            catch (CustomApiException ex)
            {
                ViewData["Message"] = ex.Errors.MyToString();
                
            }
            catch (Exception)
            {
                ViewData["Message"] = "Sorry some error occured.";

            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Remove("JwtToken");
            return RedirectToAction("Login", "Login");
        }
    }
}
