using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.UI.Exceptions;
using SocialMedia.UI.Extensions;
using SocialMedia.UI.Models;
using SocialMedia.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialMedia.UI.Controllers
{

    public class A 
    {
        public int PostId { get; set; }
        public string Description { get; set; }
    }

    public class CommentController : Controller
    {
        ICommentService _commentService;
        string token = string.Empty;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }



        [Produces("application/json")]
        public async Task<JsonResult> AddComment(CommentModel comment)
        {
            try 
            {
                token = HttpContext.Session.GetString("JwtToken");
                var id = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier).Value;
                
                comment.user_Id = id;


                await _commentService.AddComment("Comment", comment, token);

                return Json(new { result = "OK" });
            }
            catch (CustomApiException ex)
            {
                string message = ex.Errors.MyToString();
                return Json(new { result = "BAD" });

            }
            catch (Exception)
            {
                 return Json(new { result = "BAD" });

            }

          
        }
    }
}
