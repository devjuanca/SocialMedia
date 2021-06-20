using Newtonsoft.Json;
using SocialMedia.UI.Exceptions;
using SocialMedia.UI.Models;
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
    public interface ICommentService
    {
        Task AddComment(string url,CommentModel comment, string token);
    }

    public class CommentService : ICommentService
    {
        readonly HttpClient _httpClient;
        readonly IHttpClientFactory _httpClientFactory;

        public CommentService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("ClientSMApi");
        }

        public async Task AddComment(string url, CommentModel comment, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            HttpContent content = new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var exception = await response.Content.ReadFromJsonAsync<CustomApiException>();
                throw exception;
            }
        }
    }
}
