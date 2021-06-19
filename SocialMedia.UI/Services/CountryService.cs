using SocialMedia.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SocialMedia.UI.Services
{
    public interface ICountryService
    {
        Task<CountryListViewModel> GetCountries(string url);
    }

    public class CountryService : ICountryService
    {
        readonly HttpClient _httpClient;
        readonly IHttpClientFactory _httpClientFactory;

        public CountryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("ClientSMApi");
        }

        public async Task<CountryListViewModel> GetCountries(string url)
        {
            var response = await _httpClient.GetAsync($"{url}");

            if (response.IsSuccessStatusCode)
            {
                var countries = await response.Content.ReadFromJsonAsync<CountryListViewModel>();
                return countries;
            }
            else
                throw new Exception(response.ReasonPhrase);
        }
    }
}
