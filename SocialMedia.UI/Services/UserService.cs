using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocialMedia.UI.Exceptions;
using SocialMedia.UI.Models;
using SocialMedia.UI.QueryFilters;
using SocialMedia.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.UI.Services
{
    public interface IUserService
    {
        

        Task ManageUser(string url, UserModel user, IFormFile image, int accion, string token);
        Task DeleteUser(string url, string id, string token);
        Task ChangePassword(string url, ChangePasswordModel change_password, string token);
        Task ForgotPassword(string url, ForgotPasswordModel forgotPassword);

        Task<UserListViewModel> GetUsers(string url, UserQueryFilter filter, string token);
        Task<UserModel> GetUserById(string url, string id, string token);

        Task<UserDetailsViewModel> GetUserDetails(string url, string id, string token);
    }

    public class UserService : IUserService
    {
        readonly HttpClient _httpClient;
        readonly IHttpClientFactory _httpClientFactory;
        IHttpContextAccessor _accesor;
        public UserService(IHttpClientFactory httpClientFactory, IHttpContextAccessor accesor)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("ClientSMApi");
            _accesor = accesor;
        }

        public async Task<UserListViewModel> GetUsers(string url, UserQueryFilter filter, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string queryString = filter.ToString();

            var response = await _httpClient.GetAsync($"{url}{queryString}");

            if (!response.IsSuccessStatusCode)
            {
                var exception = await response.Content.ReadFromJsonAsync<CustomApiException>();
                throw exception;
            }
            return await response.Content.ReadFromJsonAsync<UserListViewModel>();
        }

        public async Task DeleteUser(string url, string id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"{url}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var exception = await response.Content.ReadFromJsonAsync<CustomApiException>();
                throw exception;
            }

        }

        public async Task ManageUser(string url, UserModel user, IFormFile image, int accion, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            byte[] profile_photo = GetImageBytes(image);
            UserModel new_user = new UserModel
            {
                Name = user.Name,
                Lastname = user.Lastname,
                Email = user.Email,
                Phone = user.Phone,
                ProfilePhoto = profile_photo,
                Password = user.Password,
                ConfirmPassword = user.ConfirmPassword,
                CountryId = user.CountryId,
                BirthDate = user.BirthDate,
                UserName = user.UserName
            };

            HttpResponseMessage response = null;
            if (accion == 0)  //Insert
            {
               
                var request = _accesor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
               

                new_user.ReturnUrl = absoluteUri + "/Login/Login";
                HttpContent content = new StringContent(JsonConvert.SerializeObject(new_user), Encoding.UTF8, "application/json");
                response = await _httpClient.PostAsync(url, content);
            }
            else if (accion == 1) //Update
            {
                new_user.Id = user.Id;
                HttpContent content = new StringContent(JsonConvert.SerializeObject(new_user), Encoding.UTF8, "application/json");
                response = await _httpClient.PutAsync(url, content);
            }

            if (!response.IsSuccessStatusCode)
            {
                var exception = await response.Content.ReadFromJsonAsync<CustomApiException>();
                throw exception;
            }
        }

        public async Task<UserModel> GetUserById(string url, string id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{url}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var userModel = await response.Content.ReadFromJsonAsync<UserObject>();
                return userModel.data;
            }
            else
                throw new Exception(response.ReasonPhrase);

        }

        public async Task<UserDetailsViewModel> GetUserDetails(string url, string id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{url}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var userModel = await response.Content.ReadFromJsonAsync<UserDetailObject>();
                return userModel.data;
            }
            else
                throw new Exception(response.ReasonPhrase);
        }

        public async Task ChangePassword(string url, ChangePasswordModel change_password, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(change_password), Encoding.UTF8,"application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var exception = await response.Content.ReadFromJsonAsync<CustomApiException>();
                throw exception;
            }
        }

        public async Task ForgotPassword(string url, ForgotPasswordModel forgotPassword)
        {
            var request = _accesor.HttpContext.Request;
            var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());

            forgotPassword.ReturnUrl = absoluteUri+"/Login/Login";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(forgotPassword), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var exception = await response.Content.ReadFromJsonAsync<CustomApiException>();
                throw exception;
            }
        }


        private static byte[] GetImageBytes(IFormFile image)
        {

            if (image == null)
            {
                return null;
            }

            if (image.Length > 2097152)
            {
                throw new CustomApiException
                {
                    Title = "Picture Error",
                    Errors = new Dictionary<string, string[]> { { "Picture Error", new string[] { "Picture is bigger than 2 mb." } } }
                };
            }

            string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };
            string ext = System.IO.Path.GetExtension(image.FileName);

            if (!permittedExtensions.Contains(ext))
            {
                throw new CustomApiException
                {
                    Title = "Picture Error",
                    Errors = new Dictionary<string, string[]> { { "Picture Error", new string[] { "Only images are allowed." } } }
                };
            }

            byte[] profile_photo = null;
            var file = image.OpenReadStream();
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                profile_photo = ms.ToArray();
            }
            return profile_photo;
        }


    }
}
