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

    public interface ICommentService
    {
        Task<CommentViewModel> GetComments(string url, int post_id, string token);
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

        public async Task<CommentViewModel> GetComments(string url, int post_id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{url}/{post_id}");

            if (!response.IsSuccessStatusCode)
            {
                var exception = await response.Content.ReadFromJsonAsync<CustomApiException>();
                throw exception;
            }
            return await response.Content.ReadFromJsonAsync<CommentViewModel>();
        }
    }
}
