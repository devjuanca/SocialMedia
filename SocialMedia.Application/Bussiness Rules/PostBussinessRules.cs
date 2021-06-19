using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.CustomExceptions;
using SocialMedia.Application.DTOs;
using SocialMedia.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Application.Bussiness_Rules
{
    public interface IPostBussinessRules
    {
        Task ApplyBussinessRules(PostCommandDTO postDTO);
    }
    public class PostBussinessRules : IPostBussinessRules
    {

        readonly SocialMediaContext _ctx;
        public PostBussinessRules(SocialMediaContext ctx)
        {
            _ctx = ctx;
        }

        public async Task ApplyBussinessRules(PostCommandDTO postDTO)
        {
            Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
            var SMUser = await _ctx.Users.FirstOrDefaultAsync(a => a.Id == postDTO.User_Id);

            if (SMUser == null)  //Rule 1: SMUser must not be null to insert a new post.
            {
                errors.Add("SMUser Not Found", new string[] { "The SMUser is not registered" });
            }
            if (postDTO.Description.Contains("sex")) // Rule 2: Word sex is not allowed.
            {
                errors.Add("Content Not Allowed", new string[] { "Content Not Allowed" });
            }
            int post_count = await _ctx.Posts.CountAsync(a => a.SmuserId == SMUser.Id);
            if (post_count <= 10 && post_count > 0) // Rule 3: Post restriction.
            {
                int post_count_last_week = await _ctx.Posts.CountAsync(a => a.SmuserId == SMUser.Id && a.Date.Value.AddDays(7) > DateTime.Today);
                if (post_count_last_week > 1)
                {
                    errors.Add("Post Count Restriction", new string[] { "You may publish only once every 7 days until you have more than 10 posts." });
                }
            }
            if (errors.Count > 0)
                throw new BussinessException("Posts Bussiness Errors", errors);
        }


    }
}

