using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Bussiness_Rules;
using SocialMedia.Application.CustomExceptions;
using SocialMedia.Application.DTOs;
using SocialMedia.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Application.Repository.Post.Posts
{
    public interface IPost_Posts
    {
        Task<PostCommandDTO> NewPost(PostCommandDTO newPost);
        Task UpdatePost(PostCommandDTO post);
        Task DeletePost(int post_id);
        
        

    }

    public class Post_Posts : IPost_Posts
    {
        readonly SocialMediaContext _ctx;
        readonly IPostBussinessRules _bussiness_rules;
        Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
        public Post_Posts(SocialMediaContext ctx, IPostBussinessRules bussiness_rules)
        {
            _ctx = ctx;
            _bussiness_rules = bussiness_rules;
        }
        public async Task<PostCommandDTO> NewPost(PostCommandDTO postDTO)
        {

            try
            {
                await _bussiness_rules.ApplyBussinessRules(postDTO);
                var newPost = await _ctx.Posts.AddAsync(new Domain.Entities.Post
                {
                    Description = postDTO.Description,
                    Date = DateTime.Now,
                    Image = postDTO.Image,
                    SmuserId = postDTO.User_Id
                });
                await _ctx.SaveChangesAsync();
                return new PostCommandDTO
                {
                    PostId = newPost.Entity.PostId,
                    Description = newPost.Entity.Description,
                    Image = newPost.Entity.Image,
                    User_Id = newPost.Entity.SmuserId
                };
            }
            catch (BussinessException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                errors.Add("ApiError", new string[] { e.Message });
                throw new ApiExceptions("Api Exception", errors);
            }
        }

        public async Task UpdatePost(PostCommandDTO updatePostDTO)
        {
            try
            {
                var post = await _ctx.Posts.FindAsync(updatePostDTO.PostId);
                if (post == null)
                {
                    errors.Add("Not Found", new string[] { "SMUsers Not Found" });
                    throw new NotFoundException("SMUsers Not Found", errors);
                }
                post.Description = updatePostDTO.Description;
                post.Image = updatePostDTO.Image;
                post.Date = DateTime.Now;

                await _ctx.SaveChangesAsync();
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

        public async Task DeletePost(int post_id)
        {
            try
            {
                var post = await _ctx.Posts.FindAsync(post_id);

                if (post == null)
                {
                    errors.Add("Not Found", new string[] { "SMUsers Not Found" });
                    throw new NotFoundException("SMUsers Not Found", errors);
                }

                _ctx.Entry(post).State = EntityState.Deleted;
                await _ctx.SaveChangesAsync();
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
