using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Bussiness_Rules;
using SocialMedia.Application.CustomExceptions;
using SocialMedia.Application.DTOs;
using SocialMedia.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Application.Repository.Comment.Posts
{
   public interface IComment_Posts
    {
        Task<CommentCommandDTO> NewComment(CommentCommandDTO newComment);
        Task UpdateComment(CommentCommandDTO comment);
        Task DeleteComment(int id);
    }


    public class Comment_Posts : IComment_Posts
    {
        readonly SocialMediaContext _ctx;
        readonly ICommentBussinessRules _commentBussinessRules;
        readonly Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
        public Comment_Posts(SocialMediaContext ctx, ICommentBussinessRules commentBussinessRules)
        {
            _ctx = ctx;
            _commentBussinessRules = commentBussinessRules;
        }
        public async Task<CommentCommandDTO> NewComment(CommentCommandDTO newComment)
        {

            try
            {
                await _commentBussinessRules.ApplyBussinessRulesToCreate(newComment);

                var createdComment = await _ctx.Comments.AddAsync(new Domain.Entities.Comment()
                {
                    PostId = newComment.PostId,
                    Description = newComment.Description,
                    SmuserId = newComment.User_Id,
                    Active = true,
                    Date = DateTime.Now
                });
                await _ctx.SaveChangesAsync();
                return new CommentCommandDTO
                {
                    Comment_Id = createdComment.Entity.CommentId,
                    Description = createdComment.Entity.Description,
                    PostId = createdComment.Entity.PostId,
                    User_Id = createdComment.Entity.Smuser.Id
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

        public async Task UpdateComment(CommentCommandDTO comment)
        {
            try
            {
                await _commentBussinessRules.ApplyBussinessRulesToModify(comment);

                var commentToModify = await _ctx.Comments.FindAsync(comment.Comment_Id);

                if (commentToModify == null)
                {
                    errors.Add("Not Found", new string[] { "SMUsers Not Found" });
                    throw new NotFoundException("SMUsers Not Found", errors);
                }

                commentToModify.Date = DateTime.Now;
                commentToModify.Description = comment.Description;

                await _ctx.SaveChangesAsync();
            }
            catch (NotFoundException e)
            {
                throw e;
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

        public async Task DeleteComment(int id)
        {
            try
            {
                var comment = await _ctx.Comments.FindAsync(id);
                if (comment == null)
                {
                    errors.Add("Not Found", new string[] { "SMUsers Not Found" });
                    throw new NotFoundException("SMUsers Not Found", errors);
                }

                _ctx.Entry(comment).State = EntityState.Deleted;
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
