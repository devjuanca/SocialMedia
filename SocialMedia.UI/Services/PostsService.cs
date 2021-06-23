using Newtonsoft.Json;
using SocialMedia.UI.Exceptions;
using SocialMedia.UI.Models;
using SocialMedia.UI.QueryFilters;
using SocialMedia.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.UI.Services
{
    public interface IPostsService
    {
        Task<PostListViewModel> GetPosts(string url, PostQueryFilter filter, string token);
        Task AddPost(string url, string token, PostCommandViewModel post);
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

        public async Task<PostListViewModel> GetPosts(string url, PostQueryFilter filter, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var query_string = filter.ToString();
            var response = await _httpClient.GetAsync($"{url}{query_string}");

            if (!response.IsSuccessStatusCode)
            {
                var exception = await response.Content.ReadFromJsonAsync<CustomApiException>();
                throw exception;
            }
            var data = await response.Content.ReadFromJsonAsync<PostListViewModel>();

            foreach (var item in data.data)
            {
                var response_comments = await _httpClient.GetAsync($"Comment/"+item.PostId);
                var data_comments = await response_comments.Content.ReadFromJsonAsync<CommentViewModel>();
                item.Comments = data_comments.data;
            }

            return data;
        }

        public async Task AddPost(string url, string token, PostCommandViewModel post)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url,content);

            if (!response.IsSuccessStatusCode)
            {
                var exception = await response.Content.ReadFromJsonAsync<CustomApiException>();
                throw exception;
            }

        }
    }
}
