using SocialMedia.Application.CustomExceptions;
using SocialMedia.Application.Entities;
using SocialMedia.Application.Enumerations;
using SocialMedia.Application.QueryFilters;
using System;
using System.Collections.Generic;

namespace SocialMedia.Application.Services
{
    public interface IUriService
    {
        string GetPaginationUri(IPagingFilter filter, string actionUr,PagingUriDirection pagingDirection);
        Uri GetCreatedEntityQueryUri(string actionUrl, string id);
        public Uri GetIdentityTokenConfirmationUri(UserToken userToken, string actionUrl, string returnUrl);

        public string BaseUri { get; }
    }

    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public string BaseUri => _baseUri;

        readonly Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
    

        public UriService(string baseUri)
        {
            _baseUri = baseUri;

        }

        




        public Uri GetIdentityTokenConfirmationUri(UserToken userToken, string actionUrl, string returnUrl)
        {
            return new Uri($"{_baseUri}{actionUrl}?UserId={userToken.UserId}&Token={userToken.Token}&ReturnUrl={returnUrl}");
        }

        public string GetPaginationUri(IPagingFilter filter, string actionUrl, PagingUriDirection pagingDirection)
        {
            try
            {
                string filterString = BuildFilterString(filter, pagingDirection);
                //return new Uri($"{_baseUri}{actionUrl}{filterString}");
                return filterString;

            }
            catch (Exception e)
            {
                errors.Add("ApiError", new string[] { e.Message });
                throw new ApiExceptions("Api Exception", errors);
            }
        }
        private static string BuildFilterString(IPagingFilter filter, PagingUriDirection pagingDirection)
        {
            string filterString = string.Empty;
            foreach (var field in filter.GetType().GetProperties())
            {
                if (field.GetValue(filter) != null)
                {
                    if (field.Name == nameof(filter.PageNumber))
                    {
                        int newPageNumber = 0;
                        if (pagingDirection == PagingUriDirection.Next)
                        {
                            newPageNumber = Convert.ToInt32(field.GetValue(filter)) + 1;
                        }
                        else
                        {
                            newPageNumber = Convert.ToInt32(field.GetValue(filter)) - 1;
                        }
                        if (filterString == String.Empty)
                        {
                            filterString = $"?{field.Name}={newPageNumber}";
                        }
                        else
                        {
                            filterString += $"&{field.Name}={newPageNumber}";
                        }
                    }
                    else
                    {

                        if (filterString == String.Empty)
                        {
                            filterString = $"?{field.Name}={field.GetValue(filter)}";
                        }
                        else
                        {
                            filterString += $"&{field.Name}={field.GetValue(filter)}";
                        }
                    }
                }
            }

            return filterString;
        }

        public Uri GetCreatedEntityQueryUri(string actionUrl, string id)
        {
            try
            {
                string baseUrl = $"{_baseUri}{actionUrl}/{id}";
                return new Uri(baseUrl);
            }
            catch (Exception e)
            {
                errors.Add("ApiError", new string[] { e.Message });
                throw new ApiExceptions("Api Exception", errors);
            }
        }


    }
}
