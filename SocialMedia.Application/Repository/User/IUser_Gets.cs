using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SocialMedia.Application.CustomExceptions;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.Enumerations;
using SocialMedia.Application.Entities;
using SocialMedia.Application.QueryFilters;
using SocialMedia.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMedia.Application.Services;
using System.Linq.Dynamic.Core;
using System.IO;
using SocialMedia.Application.ViewModels;

namespace SocialMedia.Application.Repository.SMUser.Gets
{
    public interface ISMUser_Gets
    {
        Task<PagedList<SMUserDTO>> GetAllSMUsers(SMUserQueryFilter filters);
        Task<SMUserComandDTO> GetSMUserById(string SMUser_id);
        Task<SMUserDTO> SearchSMUserByName(string name);
        Task<PagedList<SMUserDTO>> GetSMUsersWithBirthday(OnlyPagingFilter _pagingFilter);

        Task<UserDetailsViewModel> GetUserDetails(string id);

    }

    public class SMUser_Gets : ISMUser_Gets
    {
        readonly SocialMediaContext _ctx;
        readonly PagingConfiguration _pagingConfiguration;
        readonly IUriService _uriService;
        readonly private string image_directory;
        readonly private string default_picture;

        readonly Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
        public SMUser_Gets(SocialMediaContext ctx, IOptions<PagingConfiguration> pagingConfiguration, IUriService uriService)
        {
            _ctx = ctx;
            _pagingConfiguration = pagingConfiguration.Value;
            _uriService = uriService;
            image_directory = $"{_uriService.BaseUri}/MyStaticFiles/Images/";
            default_picture = $"{_uriService.BaseUri}/MyStaticFiles/Images/no_image.png";



        }

        public async Task<PagedList<SMUserDTO>> GetAllSMUsers(SMUserQueryFilter filters)
        {
            try
            {

                filters.PageSize = filters.PageSize == null ? _pagingConfiguration.DefaultPageSize : filters.PageSize;
                filters.PageNumber = filters.PageNumber == null ? _pagingConfiguration.DefaultPageNumber : filters.PageNumber;
                filters.OrderDirection = filters.OrderDirection == null ? OrderDirection.DESC : filters.OrderDirection;
                filters.OrderProperty = filters.OrderProperty == null ? SMUserOrderProperties.Name : filters.OrderProperty;

                var temp = _ctx.Users.AsNoTracking().Where(a => a.Active.Value && a.UserName != "default_admin").Select(a => new SMUserDTO
                {
                    Id = a.Id,
                    PhotoRoute = a.ProfilePhoto != null ? $"{image_directory}{a.UserName}/{a.ProfilePhoto}" : default_picture,
                    Name = a.Name,
                    Lastname = a.Lastname,
                    Phone = a.PhoneNumber,
                    Email = a.Email,
                    BirthDate = a.BirthDate,
                    Active = a.Active,
                    CountryId = a.CountryId.Value,
                    CountryName = a.Country.Name
                });
                if (filters.Name != null)
                    temp = temp.Where(a => a.Name.ToLower().Contains(filters.Name.ToLower()));
                if (filters.LastName != null)
                    temp = temp.Where(a => a.Lastname.ToLower().Contains(filters.LastName.ToLower()));
                if (filters.Email != null)
                    temp = temp.Where(a => a.Email.ToLower().Contains(filters.Email.ToLower()));
                if (filters.Phone != null)
                    temp = temp.Where(a => a.Phone.Contains(filters.Phone));
                if (filters.CountryId != null)
                {
                    temp = temp.Where(a => a.CountryId == filters.CountryId);
                }

                temp = temp.OrderBy($"{ filters.OrderProperty.Value} {filters.OrderDirection.Value}");


                //temp = OrderSMUsers(filters, temp);

                var pagedSMUsers = await PagedList<SMUserDTO>.Create(temp, filters.PageNumber.Value, filters.PageSize.Value);
                if (pagedSMUsers != null)
                    return pagedSMUsers;
                else
                {
                    errors.Add("Not Found", new string[] { "SMUsers Not Found" });
                    throw new NotFoundException("SMUsers Not Found", errors);
                }
            }
            catch (NotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                errors.Add("ApiError", new string[] { e.Message });
                throw new ApiExceptions("Api Exception", errors);
            }

        }

        //static IQueryable<SMUserDTO> OrderSMUsers(SMUserQueryFilter filters, IQueryable<SMUserDTO> temp)
        //{
        //    switch (filters.OrderProperty)
        //    {
        //        case SMUserOrderProperties.Name:
        //            {
        //                if (filters.OrderDirection == OrderDirection.ASC)
        //                {
        //                    temp = temp.AsNoTracking().OrderBy(a => a.Name);
        //                }
        //                else
        //                {
        //                    temp = temp.AsNoTracking().OrderByDescending(a => a.Name);
        //                }
        //            }
        //            break;
        //        case SMUserOrderProperties.LastName:
        //            {
        //                if (filters.OrderDirection == OrderDirection.ASC)
        //                {
        //                    temp = temp.AsNoTracking().OrderBy(a => a.Lastname);
        //                }
        //                else
        //                {
        //                    temp = temp.AsNoTracking().OrderByDescending(a => a.Lastname);
        //                }
        //            }
        //            break;
        //        case SMUserOrderProperties.Email:
        //            {
        //                if (filters.OrderDirection == OrderDirection.ASC)
        //                {
        //                    temp = temp.AsNoTracking().OrderBy(a => a.Email);
        //                }
        //                else
        //                {
        //                    temp = temp.AsNoTracking().OrderByDescending(a => a.Email);
        //                }
        //            }
        //            break;
        //        case SMUserOrderProperties.Phone:
        //            {
        //                if (filters.OrderDirection == OrderDirection.ASC)
        //                {
        //                    temp = temp.AsNoTracking().OrderBy(a => a.Phone);
        //                }
        //                else
        //                {
        //                    temp = temp.AsNoTracking().OrderByDescending(a => a.Phone);
        //                }
        //            }
        //            break;

        //        default:
        //            {
        //                temp = temp.AsNoTracking().OrderByDescending(a => a.Name);
        //            }
        //            break;
        //    }

        //    return temp;
        //}

        public async Task<SMUserComandDTO> GetSMUserById(string SMUser_id)
        {
            try
            {
                //string phisical_path = Path.GetFullPath("MyStaticFiles/Images/no_image.png");
                //byte[] default_image = File.ReadAllBytes(phisical_path);



                var SMUser = await _ctx.Users.AsNoTracking().Select(a => new SMUserComandDTO
                {
                    Id = a.Id,
                    UserName = a.UserName,
                    PhotoRoute = a.ProfilePhoto != null ? $"{image_directory}{a.UserName}/{a.ProfilePhoto}" : default_picture,
                    Name = a.Name,
                    Lastname = a.Lastname,
                    Phone = a.PhoneNumber,
                    Email = a.Email,
                    BirthDate = a.BirthDate,
                    Active = a.Active,
                    CountryId = a.CountryId.Value,
                    CountryName = a.Country.Name
                }).AsNoTracking().FirstOrDefaultAsync(a => a.Id == SMUser_id && a.Active.Value);

                if (SMUser != null)
                    return SMUser;
                else
                {
                    errors.Add("Not Found", new string[] { "SMUser Not Found" });
                    throw new NotFoundException("SMUser Not Found", errors);
                }
            }
            catch (NotFoundException e)
            {
                throw e;
            }

            catch (Exception e)
            {
                errors.Add("ApiError", new string[] { e.Message });
                throw new ApiExceptions("Api Exception", errors);
            }


        }

        public async Task<SMUserDTO> SearchSMUserByName(string name)
        {
            try
            {
                var user = await _ctx.Users.AsNoTracking().Select(a => new SMUserDTO
                {
                    Id = a.Id,
                    PhotoRoute = a.ProfilePhoto != null ? $"{image_directory}{a.UserName}/{a.ProfilePhoto}" : default_picture,
                    Name = a.Name,
                    Lastname = a.Lastname,
                    Phone = a.PhoneNumber,
                    Email = a.Email,
                    BirthDate = a.BirthDate,
                    Active = a.Active,
                    CountryId = a.CountryId.Value,
                    CountryName = a.Country.Name

                }).FirstOrDefaultAsync(a => a.Name.ToLower().Contains(name.ToLower()) && a.Active.Value);
                if (user != null)
                    return user;
                else
                {
                    errors.Add("Not Found", new string[] { "SMUser Not Found" });
                    throw new NotFoundException("SMUser Not Found", errors);
                }
            }
            catch (NotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                errors.Add("ApiError", new string[] { e.Message });
                throw new ApiExceptions("Api Exception", errors);
            }

        }

        public async Task<PagedList<SMUserDTO>> GetSMUsersWithBirthday(OnlyPagingFilter _pagingFilter)
        {
            try
            {
                _pagingFilter.PageSize = _pagingFilter.PageSize == null ? _pagingConfiguration.DefaultPageSize : _pagingFilter.PageSize;
                _pagingFilter.PageNumber = _pagingFilter.PageNumber == null ? _pagingConfiguration.DefaultPageNumber : _pagingFilter.PageNumber;

                var SMUsersWithBirthday = _ctx.Users.Where(a => a.BirthDate.Value.Month == DateTime.Now.Month && a.BirthDate.Value.Day == DateTime.Now.Day && a.Active.Value).Select(a => new SMUserDTO
                {
                    Id = a.Id,
                    PhotoRoute = a.ProfilePhoto != null ? $"{image_directory}{a.UserName}/{a.ProfilePhoto}" : default_picture,
                    Name = a.Name,
                    Lastname = a.Lastname,
                    Phone = a.PhoneNumber,
                    Email = a.Email,
                    BirthDate = a.BirthDate,
                    Active = a.Active,
                    CountryId = a.CountryId.Value,
                    CountryName = a.Country.Name

                });

                var pagedSMUsers = await PagedList<SMUserDTO>.Create(SMUsersWithBirthday, _pagingFilter.PageNumber.Value, _pagingFilter.PageSize.Value);
                if (pagedSMUsers.Count != 0)
                    return pagedSMUsers;
                else
                {
                    errors.Add("Not Found", new string[] { "SMUser Not Found" });
                    throw new NotFoundException("SMUser Not Found", errors);
                }
            }
            catch (NotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                errors.Add("ApiError", new string[] { e.Message });
                throw new ApiExceptions("Api Exception", errors);
            }
        }

        public async Task<UserDetailsViewModel> GetUserDetails(string id)
        {
            try
            {

                var user = await _ctx.Users.Include(a => a.Post).Include(a => a.Comment).Where(a=>a.Id == id).Select(a => new UserDetailsViewModel
                {
                    Name = a.Name,
                    Lastname = a.Lastname,
                    Email = a.Email,
                    Phone = a.PhoneNumber,
                    BirthDate = a.BirthDate,
                    CountryName = a.Country.Name,
                    PhotoRoute = a.ProfilePhoto != null ? $"{image_directory}{a.UserName}/{a.ProfilePhoto}" : default_picture,
                    UserName = a.UserName,
                    CantPost = a.Post.Count,
                    CantComment = a.Comment.Count

                }).FirstOrDefaultAsync();
                if (user == null)
                {
                    errors.Add("Not Found", new string[] { "SMUser Not Found" });
                    throw new NotFoundException("SMUser Not Found", errors);
                }
                   

                return user;
            }
            catch (NotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                errors.Add("ApiError", new string[] { e.Message });
                throw new ApiExceptions("Api Exception", errors);
            }
        }
    }

}
