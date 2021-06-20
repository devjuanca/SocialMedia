using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<JsonResult> GetJsonPosts()
        {
            try
            {
                token = HttpContext.Session.GetString("JwtToken");
                var data = await _postsService.GetPosts("Post", token);
                
                return Json(new { data = data, result = "Ok" });
            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message, result = "Bad" });
            }
        }
    }
}
