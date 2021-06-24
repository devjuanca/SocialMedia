using Microsoft.AspNetCore.Mvc;
using SocialMedia.Application.Entities;
using SocialMedia.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/sm/[controller]")]
    [ApiController]
    public class ConfirmAccountController : ControllerBase
    {
        readonly IAccountConfirmed _accountConfirmed;

        public ConfirmAccountController(IAccountConfirmed accountConfirmed)
        {
            _accountConfirmed = accountConfirmed;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]UserToken userToken)
        {
           await _accountConfirmed.Confirm(userToken);
            return Redirect(userToken.ReturnUrl);
        }


    }
}
