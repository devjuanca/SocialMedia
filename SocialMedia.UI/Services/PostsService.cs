using SocialMedia.UI.Exceptions;
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
    public interface IPostsService
    {
        Task<PostListViewModel> GetPosts(string url, string token);
    }

    public class PostsService : IPostsService
    {
        readonly HttpClient _httpClient;
        readonly IHttpClientFactory _httpClientFactory;

        public PostsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("ClientSMApi");
        }

        public async Task<PostListViewModel> GetPosts(string url, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{url}");

            if (!response.IsSuccessStatusCode)
            {
                var exception = await response.Content.ReadFromJsonAsync<CustomApiException>();
                throw exception;
            }
            return await response.Content.ReadFromJsonAsync<PostListViewModel>();
        }
    }
}
