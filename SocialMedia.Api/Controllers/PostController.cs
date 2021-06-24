using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Api.Response;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.Enumerations;
using SocialMedia.Application.ExceptionsResult;
using SocialMedia.Application.QueryFilters;
using SocialMedia.Application.Repository.Post.Gets;
using SocialMedia.Application.Repository.Post.Posts;
using SocialMedia.Application.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/sm/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        IPost_Gets _posts;
        IPost_Posts _posts_P;
        IUriService _uriService;
      
        public PostController(IPost_Gets post, IPost_Posts posts_P, IUriService uriService)
        {
            _posts = post;
            _posts_P = posts_P;
            _uriService = uriService;
        }

        /// <summary>
        ///  Retrive all posts with filter options, paginated and ordered according to parameters.
        /// </summary>
        /// <param name="filters">Filtered through the object PostQueryFilter</param>
        /// <returns></returns>
        /// 
      
        [HttpGet(Name =nameof(AllPosts))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponseWithMeta<IEnumerable<PostDTO>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> AllPosts([FromQuery] PostQueryFilter filters)
        {
            var posts = await _posts.GetAllPosts(filters);

            var metadata = new Metadata
            {
                TotalPages = posts.TotalPages,
                ItemsCount = posts.ItemsCount,
                PageSize = posts.PageSize,
                CurrentPage = posts.CurrentPage,
                HasPrevious = posts.HasPrevious,
                HasNext = posts.HasNext,
                NextPageUrl =(posts.HasNext)? _uriService.GetPaginationUri(filters, Url.RouteUrl(nameof(AllPosts)),PagingUriDirection.Next).ToString():"",
                PreviousPageUrl =(posts.HasPrevious)? _uriService.GetPaginationUri(filters, Url.RouteUrl(nameof(AllPosts)),PagingUriDirection.Previous).ToString():""

            };
          
            var response = new ApiResponseWithMeta<IEnumerable<PostDTO>>(posts) 
            { Meta = metadata };
            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(response);
        }

        /// <summary>
        /// Retrive the post by passing the Id as parameter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
     
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiSingleResponse<PostDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> PostById(int id)
        {
            var response = new ApiSingleResponse<PostDTO>(await _posts.GetPostById(id));
            return Ok(response);
        }

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="newPost"></param>
        /// <returns></returns>
        /// 
      
        [HttpPost(Name = nameof(CreateNewPost))]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(ApiSingleResponse<PostCommandDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type =typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> CreateNewPost(PostCommandDTO newPost)
        {
           var createdPost =  await _posts_P.NewPost(newPost);
            return Created(_uriService.GetCreatedEntityQueryUri(Url.RouteUrl(nameof(CreateNewPost)).ToString(), createdPost.PostId.ToString()), new ApiSingleResponse<PostCommandDTO>(createdPost));
            
        }

        /// <summary>
        /// Modifies a post. Allowed to modify only Description and Image. The Date will be updated with the current date.
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        /// 
   
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> UpdatePost(PostCommandDTO post)
        {
            await _posts_P.UpdatePost(post);
            return NoContent();
        }

        /// <summary>
        /// Delete a post by its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _posts_P.DeletePost(id);
            return NoContent();
        }

       
  
    }
}


