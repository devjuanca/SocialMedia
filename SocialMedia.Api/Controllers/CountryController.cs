using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Response;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.Repository.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
   
    [Produces("application/json")]
    [Route("api/sm/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        ICountry _country;

        public CountryController(ICountry country)
        {
            _country = country;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiSingleResponse<IEnumerable<CountryDTO>>))]
        public async Task<IActionResult> GetCountries()
        {
            var countries = new ApiSingleResponse<IEnumerable<CountryDTO>>(await _country.GetCountries());
            return Ok(countries);
        }
    }
}
