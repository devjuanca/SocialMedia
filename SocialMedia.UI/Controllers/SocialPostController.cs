using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.UI.Exceptions;
using SocialMedia.UI.Extensions;
using SocialMedia.UI.QueryFilters;
using SocialMedia.UI.Services;
using SocialMedia.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialMedia.UI.Controllers
{
    public class SocialPostController : Controller
    {
        string token = string.Empty;
        IPostsService _postsService;

        public SocialPostController(IPostsService postsService)
        {
            _postsService = postsService;
        }


        public IActionResult GetPosts()
        {
            return View();
        }

        [Produces("application/json")]
        public async Task<JsonResult> GetJsonPosts([FromQuery] PostQueryFilter filter)
        {
            try
            {
                token = HttpContext.Session.GetString("JwtToken");
                var data = await _postsService.GetPosts("Post",filter, token);
                
                return Json(new { data = data, result = "Ok" });
            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message, result = "Bad" });
            }
        }

        public IActionResult NewPost()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewPost(PostCommandViewModel post)
        {
            try
            {
                token = HttpContext.Session.GetString("JwtToken");

                string id = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier).Value;
                post.User_Id = id;

                await _postsService.AddPost("Post",token, post);
                return RedirectToAction("GetPosts");
                
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
