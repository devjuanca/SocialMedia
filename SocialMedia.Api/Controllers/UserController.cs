using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Api.Response;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.Enumerations;
using SocialMedia.Application.ExceptionsResult;
using SocialMedia.Application.Entities;
using SocialMedia.Application.QueryFilters;
using SocialMedia.Application.Repository.SMUser.Gets;
using SocialMedia.Application.Repository.SMUser.Posts;
using SocialMedia.Application.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SocialMedia.Application.ViewModels;

namespace SocialMedia.Api.Controllers
{
    //[Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    
    public class SMUserController : ControllerBase
    {
        ISMUser_Gets _SMUser_Gets;
        ISMUser_Posts _SMUser_Posts;
        IUriService _uriService;
        

        public SMUserController(ISMUser_Gets SMUser_Gets, ISMUser_Posts SMUser_Posts, IUriService uriService)
        {
            _SMUser_Gets = SMUser_Gets;
            _SMUser_Posts = SMUser_Posts;
            _uriService = uriService;
      
        }
        /// <summary>
        ///  Retrive all SMUsers with filter options, paginated and ordered according to parameters.
        /// </summary>
        /// <param name="filters">Filtered through the object PostQueryFilter</param>
        /// <returns></returns>
        ///

        [Authorize]
        [HttpGet(Name = nameof(AllSMUsers))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponseWithMeta<IEnumerable<SMUserDTO>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> AllSMUsers([FromQuery] SMUserQueryFilter filters)
        {
            var SMUsers = await _SMUser_Gets.GetAllSMUsers(filters);

            var metadata = new Metadata
            {
                TotalPages = SMUsers.TotalPages,
                ItemsCount = SMUsers.ItemsCount,
                PageSize = SMUsers.PageSize,
                CurrentPage = SMUsers.CurrentPage,
                HasPrevious = SMUsers.HasPrevious,
                HasNext = SMUsers.HasNext,
                NextPageUrl = (SMUsers.HasNext) ? _uriService.GetPaginationUri(filters, Url.RouteUrl(nameof(AllSMUsers)), PagingUriDirection.Next).ToString() : "",
                PreviousPageUrl = (SMUsers.HasPrevious) ? _uriService.GetPaginationUri(filters, Url.RouteUrl(nameof(AllSMUsers)), PagingUriDirection.Previous).ToString() : ""

            };

            var response = new ApiResponseWithMeta<IEnumerable<SMUserDTO>>(SMUsers)
            { Meta = metadata };
            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }
        /// <summary>
        /// Gets all SMUsers who have birthdays today.
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        ///  
        [Authorize]
        [HttpGet]
        [Route("SMUsersWithBirthday", Name = nameof(AllSMUsersWithBirthday))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponseWithMeta<IEnumerable<SMUserDTO>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> AllSMUsersWithBirthday([FromQuery] OnlyPagingFilter filters)
        {
            var SMUsers = await _SMUser_Gets.GetSMUsersWithBirthday(filters);

            var metadata = new Metadata
            {
                TotalPages = SMUsers.TotalPages,
                ItemsCount = SMUsers.ItemsCount,
                PageSize = SMUsers.PageSize,
                CurrentPage = SMUsers.CurrentPage,
                HasPrevious = SMUsers.HasPrevious,
                HasNext = SMUsers.HasNext,
                NextPageUrl = (SMUsers.HasNext) ? _uriService.GetPaginationUri(filters, Url.RouteUrl(nameof(AllSMUsersWithBirthday)),PagingUriDirection.Next).ToString() : "",
                PreviousPageUrl = (SMUsers.HasPrevious) ? _uriService.GetPaginationUri(filters, Url.RouteUrl(nameof(AllSMUsersWithBirthday)),PagingUriDirection.Previous).ToString() : ""

            };

            var response = new ApiResponseWithMeta<IEnumerable<SMUserDTO>>(SMUsers)
            { Meta = metadata };
            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }



        /// <summary>
        /// Get the SMUser by passing the Id as parameter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiSingleResponse<SMUserDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> SMUserById(string id)
        {
            var response = new ApiSingleResponse<SMUserComandDTO>(await _SMUser_Gets.GetSMUserById(id));
            return Ok(response);
        }
        /// <summary>
        /// Get the SMUser by passing the Name as parameter.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// 
        [Authorize]
        [HttpGet("GetSMUserByName{name}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiSingleResponse<SMUserDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> SMUserByName(string name)
        {
            var response = new ApiSingleResponse<SMUserDTO>(await _SMUser_Gets.SearchSMUserByName(name));
            return Ok(response);
        }


        [Authorize]
        [HttpGet("GetSMUserDetails/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiSingleResponse<UserDetailsViewModel>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> GetSMUserDetails(string id)
        {
            var response = new ApiSingleResponse<UserDetailsViewModel>(await _SMUser_Gets.GetUserDetails(id));
            return Ok(response);
        }






        /// <summary>
        /// Creates a new SMUser.
        /// </summary>
        /// <param name="SMUserDTO"></param>
        /// <returns></returns>
        [HttpPost(Name = nameof(CreateNewSMUser))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiSingleResponse<SMUserComandDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> CreateNewSMUser(SMUserComandDTO SMUserDTO)
        {
            SMUserDTO createdSMUser = await _SMUser_Posts.NewSMUser(SMUserDTO);
            Uri SMUserLink = _uriService.GetCreatedEntityQueryUri(Url.RouteUrl(nameof(CreateNewSMUser)), createdSMUser.Id.ToString());
            return Created(SMUserLink.ToString(), new ApiSingleResponse<SMUserDTO>(createdSMUser));
        }

        /// <summary>
        /// Delete a SMUser by its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> DeleteSMUser(string id)
        {
            await _SMUser_Posts.DeleteSMUser(id);
            return NoContent();
        }

        /// <summary>
        /// Modifies an SMUser data.
        /// </summary>
        /// <param name="SMUser"></param>
        /// <returns></returns>
        /// 
       
        [Authorize]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> UpdateSMUser(SMUserComandDTO SMUser)
        {
            await _SMUser_Posts.UpdateSMUser(SMUser);
            return NoContent();
        }
        /// <summary>
        /// Disable an SMUser from Api.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpPut("DisableSMUser/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> DisableSMUser(string id)
        {
           await _SMUser_Posts.DisableSMUser(id);
            return NoContent();
        }
        /// <summary>
        /// Enables an SMUser from Api.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpPut("EnableSMUser/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> EnableSMUser(string id)
        {
            await _SMUser_Posts.EnableSMUser(id);
            return NoContent();
        }

        
    }
}
