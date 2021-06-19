using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.CustomExceptions;
using SocialMedia.Application.DTOs;
using SocialMedia.Domain.Entities.CustomEntities;
using SocialMedia.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Application.Repository.Comment.Gets
{
   public interface IComment_Gets
    {
        Task<IEnumerable<CommentDTO>> GetCommentsFromPost(int post_id);
     
    }

    public class Comment_Gets : IComment_Gets
    {
        readonly SocialMediaContext _ctx;
        readonly Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
        public Comment_Gets(SocialMediaContext ctx)
        {
            _ctx = ctx;
        }



        public async Task<IEnumerable<CommentDTO>> GetCommentsFromPost(int post_id)
        {
            List<CommentDTO> comments = new List<CommentDTO>();
            try
            {
                comments = await _ctx.CommentsProc
                  .FromSqlRaw<S_CommentsFromPost>("S_CommentsFromPost @Id", new SqlParameter[] { new SqlParameter("@Id", post_id) }).AsNoTracking()
                  .Select(a => new CommentDTO
                  {
                      Comment_Id = a.Comment_Id,
                      Date = a.Date,
                      Description = a.Description,
                      User_Id = a.User_Id,
                      SMUserName = a.Name,
                      PostId = a.PostId
                  })
                  .ToListAsync();
                if (comments.Count > 0)
                    return comments;
                else
                {
                    errors.Add("Not Found", new string[] { "Comment Not Found" });
                    throw new NotFoundException("Comment Not Found", errors);
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
