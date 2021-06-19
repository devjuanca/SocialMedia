using Microsoft.AspNetCore.Mvc;
using SocialMedia.Application.Entities;
using SocialMedia.Application.Repository.Authentication;
using SocialMedia.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SocialMedia.Api.Response;
using Microsoft.Extensions.Configuration;

namespace SocialMedia.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        IAuthentication _authentication;
        ITokenGenerator _tokenGenerator;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IAuthentication authentication, ITokenGenerator tokenGenerator, IConfiguration configuration)
        {
            _authentication = authentication;
            _tokenGenerator = tokenGenerator;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiSingleResponse<string>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            string issuer = _configuration["Authentication:Issuer"];
            string audience = _configuration["Authentication:Audience"];
            string secretKey = _configuration["Authentication:SecretKey"];

            var user = await _authentication.Login(userLogin);
            var data_token = await _tokenGenerator.GenerateToken(user,secretKey,issuer,audience);
            var response = new ApiSingleResponse<DataToken>(data_token);
           
            return Ok(response);

        }
    }
}
