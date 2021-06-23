using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SocialMedia.Application.CustomExceptions;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.Entities;
using SocialMedia.Application.Enumerations;
using SocialMedia.Application.QueryFilters;
using SocialMedia.Application.Repository.SMUser.Posts;
using SocialMedia.Application.ViewModels;
using SocialMedia.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Application.Repository.AdminUser
{
    public interface IAdminUser
    {
        Task CreateNewAdmin(SMUserComandDTO SMUserDTO, string returnUrl);
        Task ModifyRolesToUser(RolesInUser param);
        Task<PagedList<UsersWithRolesVM>> GetAllAdmins(SMUserQueryFilter filter);
    }


    public class AdminUser : IAdminUser
    {
        readonly ISMUser_Posts _user;
        readonly UserManager<SocialMedia.Domain.Entities.SMUser> _userManager;
        readonly SocialMediaContext _ctx;
        readonly PagingConfiguration _pagingConfiguration;
        readonly Dictionary<string, string[]> errors = new Dictionary<string, string[]>();

        public AdminUser(ISMUser_Posts user, UserManager<SocialMedia.Domain.Entities.SMUser> userManager, IOptions<PagingConfiguration> pagingConfiguration, SocialMediaContext ctx)
        {
            _user = user;
            _userManager = userManager;
            _pagingConfiguration = pagingConfiguration.Value;
            _ctx = ctx;
        }

        public async Task CreateNewAdmin(SMUserComandDTO SMUserDTO, string returnUrl)
        {
            try
            {
                SMUserComandDTO new_user = await _user.NewSMUser(SMUserDTO);
                SocialMedia.Domain.Entities.SMUser user = await _userManager.FindByIdAsync(new_user.Id);
                var result = await _userManager.AddToRoleAsync(user, "Admin");
                if (!result.Succeeded)
                {
                    throw new ApiExceptions("An error ocured when creating a new user", FromIdentityResultError(result.Errors));
                }
            }
            catch (ApiExceptions e)
            {
                throw new ApiExceptions(e.Message, e.Errors);
            }

        }

        public async Task<PagedList<UsersWithRolesVM>> GetAllAdmins(SMUserQueryFilter filters)
        {
            try
            {
                filters.PageSize = filters.PageSize == null ? _pagingConfiguration.DefaultPageSize : filters.PageSize;
                filters.PageNumber = filters.PageNumber == null ? _pagingConfiguration.DefaultPageNumber : filters.PageNumber;
                IQueryable<UsersWithRolesVM> temp = null;

                await Task.Run(() =>
                {
                    temp = from t in _ctx.UserRoles
                           join u in _ctx.Users on t.UserId equals u.Id
                           select new UsersWithRolesVM
                           {
                               Name = u.Name,
                               Lastname = u.Lastname,
                               UserName = u.UserName,
                               Email = u.Email,
                               Phone = u.PhoneNumber,
                               Roles = (from r in _ctx.Roles where r.Id == t.RoleId select r.Name).ToList()
                           };
                });


                if (filters.Name != null)
                    temp = temp.Where(a => a.Name.ToLower().Contains(filters.Name.ToLower()));
                if (filters.LastName != null)
                    temp = temp.Where(a => a.Lastname.ToLower().Contains(filters.Name.ToLower()));
                if (filters.Email != null)
                    temp = temp.Where(a => a.Email.ToLower().Contains(filters.Email.ToLower()));
                if (filters.Phone != null)
                    temp = temp.Where(a => a.Phone.Contains(filters.Phone));

                temp = OrderSMUsers(filters, temp);

                var pagedSMUsers = await PagedList<UsersWithRolesVM>.Create(temp, filters.PageNumber.Value, filters.PageSize.Value);
                if (pagedSMUsers.Count > 0)
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


        static IQueryable<UsersWithRolesVM> OrderSMUsers(SMUserQueryFilter filters, IQueryable<UsersWithRolesVM> temp)
        {
            switch (filters.OrderProperty)
            {
                case SMUserOrderProperties.Name:
                    {
                        if (filters.OrderDirection == OrderDirection.ASC)
                        {
                            temp = temp.AsNoTracking().OrderBy(a => a.Name);
                        }
                        else
                        {
                            temp = temp.AsNoTracking().OrderByDescending(a => a.Name);
                        }
                    }
                    break;
                case SMUserOrderProperties.LastName:
                    {
                        if (filters.OrderDirection == OrderDirection.ASC)
                        {
                            temp = temp.AsNoTracking().OrderBy(a => a.Lastname);
                        }
                        else
                        {
                            temp = temp.AsNoTracking().OrderByDescending(a => a.Lastname);
                        }
                    }
                    break;
                case SMUserOrderProperties.Email:
                    {
                        if (filters.OrderDirection == OrderDirection.ASC)
                        {
                            temp = temp.AsNoTracking().OrderBy(a => a.Email);
                        }
                        else
                        {
                            temp = temp.AsNoTracking().OrderByDescending(a => a.Email);
                        }
                    }
                    break;
                case SMUserOrderProperties.Phone:
                    {
                        if (filters.OrderDirection == OrderDirection.ASC)
                        {
                            temp = temp.AsNoTracking().OrderBy(a => a.Phone);
                        }
                        else
                        {
                            temp = temp.AsNoTracking().OrderByDescending(a => a.Phone);
                        }
                    }
                    break;

                default:
                    {
                        temp = temp.AsNoTracking().OrderByDescending(a => a.Name);
                    }
                    break;
            }

            return temp;
        }

        public async Task ModifyRolesToUser(RolesInUser param)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(param.UserName);

                if (user == null)
                {
                    throw new ApiExceptions("The user does't exist", null);
                }

                IdentityResult remove_result = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

                IdentityResult addroles_result = await _userManager.AddToRolesAsync(user, param.Roles);

                if (!remove_result.Succeeded)
                {
                    throw new ApiExceptions("Some errors occured deleting existing roles from the user", FromIdentityResultError(remove_result.Errors));
                }
                if (!addroles_result.Succeeded)
                {
                    throw new ApiExceptions("Some errors occured adding existing roles from the user", FromIdentityResultError(addroles_result.Errors));
                }
            }
            catch (Exception ex)
            {
                if (ex is ApiExceptions)
                {
                    throw ex;
                }
                else
                    throw new ApiExceptions("Some error occured.", null);
            }
        }



        private Dictionary<string, string[]> FromIdentityResultError(IEnumerable<IdentityError> identityErrors)
        {
            Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
            List<string> iErrors = new List<string>();
            foreach (var item in identityErrors)
            {
                iErrors.Add(item.Description);
            }
            errors.Add("User Creating Errors", iErrors.ToArray());

            return errors;
        }
    }
}
