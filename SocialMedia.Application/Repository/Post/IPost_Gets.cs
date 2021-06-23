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
using System.Linq.Dynamic.Core;
using SocialMedia.Application.Services;

namespace SocialMedia.Application.Repository.Post.Gets
{
   public interface IPost_Gets
    {
        Task<PagedList<PostDTO>> GetAllPosts(PostQueryFilter filters);
        Task<PostDTO> GetPostById(int post_id);
    
    }


    public class Post_Gets : IPost_Gets
    {
        readonly SocialMediaContext _ctx;
        readonly PagingConfiguration _pagingConfiguration;
        readonly Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
        readonly IUriService _uriService;
        readonly private string image_directory;
        readonly private string default_picture;
        public Post_Gets(SocialMediaContext ctx, IOptions<PagingConfiguration> pagingConfiguration, IUriService uriService)
        {
            _ctx = ctx;
            _pagingConfiguration = pagingConfiguration.Value;
            _uriService = uriService;
            image_directory = $"{_uriService.BaseUri}/MyStaticFiles/Images/";
            default_picture = $"{_uriService.BaseUri}/MyStaticFiles/Images/no_image.png";
        }
        //IOptions es una interfaz usada para inyectar un servicio que obtiene datos de una seccion del appsettings 
        //y se registra en el startup mediante services.Configure().
        public async Task<PagedList<PostDTO>> GetAllPosts(PostQueryFilter filters)
        {
            try
            {
                filters.PageSize = filters.PageSize == null ? _pagingConfiguration.DefaultPageSize : filters.PageSize;
                filters.PageNumber = filters.PageNumber == null ? _pagingConfiguration.DefaultPageNumber : filters.PageNumber;
                //Valores por defecto en seccion Pagination del appsettings.
                filters.OrderDirection = filters.OrderDirection == null ? OrderDirection.DESC : filters.OrderDirection;
                filters.OrderProperty = filters.OrderProperty == null ? PostOrderProperties.Date : filters.OrderProperty;

                var test = _ctx.Posts;

                var temp = _ctx.Posts.Select(a => new PostDTO
                {
                    PostId = a.PostId,
                    Date = a.Date,
                    Description = a.Description,
                    Image = a.Image,
                    User_Id = a.SmuserId,
                    SMUserName = a.Smuser.Name,
                    Name = a.Smuser.Name +" " + a.Smuser.Lastname,
                    ProfilePhoto = a.Smuser.ProfilePhoto != null ? $"{image_directory}{a.Smuser.UserName}/{a.Smuser.ProfilePhoto}" : default_picture
                });
                if (filters.Id != null)
                {
                    temp = temp.Where(a => a.User_Id == filters.Id);
                }

                if (filters.UserId != null)
                {
                    temp = temp.Where(a => a.User_Id == filters.UserId);
                }

                if (filters.DescriptionSearch != null)
                {
                    temp = temp.Where(a => a.Description.ToLower().Contains(filters.DescriptionSearch.ToLower()));
                }
                if (filters.Date != null)
                {
                    temp = temp.Where(a => a.Date.Value.Year == filters.Date.Value.Year && a.Date.Value.Month == filters.Date.Value.Month && a.Date.Value.Day == filters.Date.Value.Day);
                }
                //Conformo la query con todos los filtros, luego la hago No Tracking y se aplica la ordenación.
                //EL Create del Paged List devuelve de forma asincrona las entidades paginadas.
                temp = temp.OrderBy($"{ filters.OrderProperty.Value} {filters.OrderDirection.Value}");

                //temp = OrderPosts(filters, temp);
                // aqui ya viene el listado de items de manera asincrona.
                var pagedPosts = await PagedList<PostDTO>.Create(temp, filters.PageNumber.Value, filters.PageSize.Value);

                if (pagedPosts.Count > 0)
                    return pagedPosts;
                else
                {
                    errors.Add("Not Found", new string[] { "Posts Not Found" });
                    throw new NotFoundException("Posts Not Found", errors);
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

        //static IQueryable<PostDTO> OrderPosts(PostQueryFilter filters, IQueryable<PostDTO> temp)
        //{


        //    switch (filters.OrderProperty)
        //    {
        //        case PostOrderProperties.SMUserName:
        //            {
        //                if (filters.OrderDirection == OrderDirection.ASC)
        //                {
        //                    temp = temp.AsNoTracking().OrderBy(a => a.SMUserName);
        //                }
        //                else
        //                {
        //                    temp = temp.AsNoTracking().OrderByDescending(a => a.SMUserName);
        //                }
        //            }
        //            break;
        //        case PostOrderProperties.Date:
        //            {
        //                if (filters.OrderDirection == OrderDirection.ASC)
        //                {
        //                    temp = temp.AsNoTracking().OrderBy(a => a.Date);
        //                }
        //                else
        //                {
        //                    temp = temp.AsNoTracking().OrderByDescending(a => a.Date);
        //                }
        //            }
        //            break;
        //        default:
        //            {
        //                temp = temp.AsNoTracking().OrderByDescending(a => a.Date);
        //            }
        //            break;
        //    }

        //    return temp;
        //}

        public async Task<PostDTO> GetPostById(int post_id)
        {
            try
            {
                var post = await _ctx.Posts.Select(a => new PostDTO
                {
                    PostId = a.PostId,
                    Date = a.Date,
                    Description = a.Description,
                    Image = a.Image,
                    User_Id = a.SmuserId,
                    SMUserName = a.Smuser.Name
                }).AsNoTracking().FirstOrDefaultAsync(a => a.PostId == post_id);

                if (post != null)
                    return post;
                else
                {
                    errors.Add("Not Found", new string[] { "Post Not Found" });
                    throw new NotFoundException("Post Not Found", errors);
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

    }

}
