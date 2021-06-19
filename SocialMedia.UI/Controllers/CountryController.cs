using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.Controllers
{
   
    public class CountryController : Controller
    {
        ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<JsonResult> GetCountries()
        {
           
            var countries = await _countryService.GetCountries("Country");
            return Json(new {data = countries });
        }
    }
}
