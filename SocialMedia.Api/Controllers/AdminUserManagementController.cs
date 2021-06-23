using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Response;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.Enumerations;
using SocialMedia.Application.ExceptionsResult;
using SocialMedia.Application.Entities;
using SocialMedia.Application.QueryFilters;
using SocialMedia.Application.Repository.AdminUser;
using SocialMedia.Application.Services;
using SocialMedia.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Authorize(Roles ="Admin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserManagementController : Controller
    {

        readonly IAdminUser _adminUser;
        readonly IUriService _uriService;

        public AdminUserManagementController(IAdminUser adminUser, IUriService uriService)
        {
            _adminUser = adminUser;
            _uriService = uriService;
        }

        [HttpPost("CreateNewAdmin")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateNewAdmin(SMUserComandDTO SMUserDTO, string returnUrl)
        {
            await _adminUser.CreateNewAdmin(SMUserDTO, returnUrl);
            return Ok();
        }

        [HttpPost("ModifyRoles")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ModifyRolesToUser(RolesInUser param)
        {
            await _adminUser.ModifyRolesToUser(param);
            return Ok();
        }

 
        [HttpGet(Name = nameof(GetAllAdmins))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponseWithMeta<PagedList<UsersWithRolesVM>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(GenericExceptionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(GenericExceptionResult))]
        public async Task<IActionResult> GetAllAdmins([FromQuery]SMUserQueryFilter filters)
        {
            var SMUsers = await _adminUser.GetAllAdmins(filters);
            var metadata = new Metadata
            {
                TotalPages = SMUsers.TotalPages,
                ItemsCount = SMUsers.ItemsCount,
                PageSize = SMUsers.PageSize,
                CurrentPage = SMUsers.CurrentPage,
                HasPrevious = SMUsers.HasPrevious,
                HasNext = SMUsers.HasNext,
                NextPageUrl = (SMUsers.HasNext) ? _uriService.GetPaginationUri(filters, Url.RouteUrl(nameof(GetAllAdmins)), PagingUriDirection.Next).ToString() : "",
                PreviousPageUrl = (SMUsers.HasPrevious) ? _uriService.GetPaginationUri(filters, Url.RouteUrl(nameof(GetAllAdmins)), PagingUriDirection.Next).ToString() : ""

            };
            var response = new ApiResponseWithMeta<IEnumerable<UsersWithRolesVM>>(SMUsers)
            { Meta = metadata };

            return Ok(response);
        }


    }
}
