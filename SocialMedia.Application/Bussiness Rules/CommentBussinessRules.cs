using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.CustomExceptions;
using SocialMedia.Application.DTOs;
using SocialMedia.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Application.Bussiness_Rules
{
    public interface ICommentBussinessRules
    {
        Task ApplyBussinessRulesToCreate(CommentCommandDTO comment);
        Task ApplyBussinessRulesToModify(CommentCommandDTO comment);

    }
    public class CommentBussinessRules : ICommentBussinessRules
    {
        readonly SocialMediaContext _ctx;
        public CommentBussinessRules(SocialMediaContext ctx)
        {
            _ctx = ctx;
        }

        public async Task ApplyBussinessRulesToCreate(CommentCommandDTO comment)
        {
            Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
            var SMUser = await _ctx.Users.FirstOrDefaultAsync(a => a.Id == comment.User_Id);

            if (SMUser == null)  //Rule 1: SMUser must not be null to insert a new comment
            {
                errors.Add("SMUser Not Found", new string[] { "The SMUser is not registered" });
            }
            if (comment.Description.Contains("sex")) // Rule 2: Word sex is not allowed.
            {
                errors.Add("Content Not Allowed", new string[] { "Content Not Allowed" });
            }
            if (errors.Count > 0)
                throw new BussinessException("Comments Bussiness Errors", errors);


        }

        public async Task ApplyBussinessRulesToModify(CommentCommandDTO comment)
        {
            Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
            var commentStored = await _ctx.Comments.FirstOrDefaultAsync(a => a.CommentId == comment.Comment_Id);
            if (commentStored.SmuserId != comment.User_Id) // Rule 1: Only the SMUser who create the comment can modify it.
            {
                errors.Add("The SMUser is Different", new string[] { "The SMUser who sent the update request is different that the comment creator." });
            }
            if (errors.Count > 0)
                throw new BussinessException("Comments Bussiness Errors", errors);

        }
    }
}
