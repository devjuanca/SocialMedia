using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.Controllers
{
    public class CommentController : Controller
    {
        ICommentService _commentService;
        string token = string.Empty;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<JsonResult> GetJsonComments(int post_id)
        {
            try
            {
                token = HttpContext.Session.GetString("JwtToken");
                var data = await _commentService.GetComments("Comment",post_id, token);

                return Json(new { data = data, result = "Ok" });
            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message, result = "Bad" });
            }
        }
    }
}
