using SocialMedia.UI.DTO;
using SocialMedia.UI.Exceptions;
using SocialMedia.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SocialMedia.UI.Services
{
    public interface ILoginService
    {
        Task<TokenResult> Login(LoginModel loginViewModel);
    }

    public class LoginService : ILoginService
    {
        readonly HttpClient _httpClient;
        readonly IHttpClientFactory _httpClientFactory;
        public LoginService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("ClientSMApi");
        }

        public async Task<TokenResult> Login(LoginModel loginViewModel)
        {
            var response = await _httpClient.PostAsJsonAsync<LoginModel>("Authentication/login", loginViewModel);
            if (response.IsSuccessStatusCode)
            {
                var data_token = await response.Content.ReadFromJsonAsync<TokenResult>();
                return data_token;
            }
            else
            {
                var exception = await response.Content.ReadFromJsonAsync<CustomApiException>();
                throw exception;
            }
        }

    }
}
