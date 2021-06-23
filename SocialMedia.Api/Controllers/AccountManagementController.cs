using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.Entities;
using SocialMedia.Application.Services;
using SocialMedia.Domain.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountManagementController : Controller
    {
        readonly IChangePasswordService _changePasswordService;
        readonly IResetPasswordService _resetPasswordService;

        public AccountManagementController(IChangePasswordService changePasswordService, IResetPasswordService resetPasswordService)
        {
            _changePasswordService = changePasswordService;
            _resetPasswordService = resetPasswordService;
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordEntity changePasswordEntity)
        {
            await _changePasswordService.ChangePassword(changePasswordEntity);
            return Ok("Password Changed.");
        }
     
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(PasswordResetDTO passwordResetDTo)
        {
            await _resetPasswordService.SendTokenToUser(passwordResetDTo);
            return Ok();
        }
        
        [HttpGet("PerformResetPassword")]
        public async Task<IActionResult> PerformResetPassword([FromQuery] UserToken userToken)
        {
            await _resetPasswordService.ResetPassword(userToken);
            return Redirect(userToken.ReturnUrl);
        }
    }
}
