using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Response;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.ExceptionsResult;
using SocialMedia.Application.Repository.Comment.Gets;
using SocialMedia.Application.Repository.Comment.Posts;
using SocialMedia.Application.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        IComment_Gets _comment_Gets;
        IComment_Posts _comment_Posts;
        IUriService _uriService;

        public CommentController(IComment_Gets comment_Gets, IComment_Posts comment_Posts, IUriService uriService)
        {
            _comment_Gets = comment_Gets;
            _comment_Posts = comment_Posts;
            _uriService = uriService;
        }

        /// <summary>
        /// Gets all the comments of a specific post.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiSingleResponse<CommentDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> GetCommentsFromPost(int id)
        {
            var response = new ApiSingleResponse< IEnumerable<CommentDTO>>(await _comment_Gets.GetCommentsFromPost(id));
            return Ok(response);
        }
        /// <summary>
        /// Creates a new comment asociated to a post.
        /// </summary>
        /// <param name="newComment"></param>
        /// <returns></returns>
        /// 
     
        [HttpPost(Name = nameof(CreateNewComment))]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(ApiSingleResponse<CommentCommandDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> CreateNewComment(CommentCommandDTO newComment)
        {
            var createdComment = await _comment_Posts.NewComment(newComment);
            return Created(_uriService.GetCreatedEntityQueryUri(Url.RouteUrl(nameof(CreateNewComment)).ToString(), createdComment.Comment_Id.ToString()), new ApiSingleResponse<CommentCommandDTO>(createdComment));
        }
        /// <summary>
        /// Updates a specific comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        /// 
       
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> UpdateComment(CommentCommandDTO comment)
        {
            await _comment_Posts.UpdateComment(comment);
            return NoContent();
        }
        /// <summary>
        /// Deletes a comment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _comment_Posts.DeleteComment(id);
            return NoContent();
        }


    }
}
