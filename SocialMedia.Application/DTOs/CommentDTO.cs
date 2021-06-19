using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Application.DTOs
{
   public class CommentDTO
    {
        public int Comment_Id { get; set; }
        public int PostId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public string User_Id { get; set; }
        public string SMUserName { get; set; }
        
    }
    public class CommentCommandDTO
    {
        public int Comment_Id { get; set; }
        public string User_Id { get; set; }
        public int PostId { get; set; }
        public string Description { get; set; }
        
    }
}
