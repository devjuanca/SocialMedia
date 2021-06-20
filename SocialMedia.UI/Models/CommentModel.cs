using System;

namespace SocialMedia.UI.Models
{

    public class CommentModel
    {
        public int comment_Id { get; set; }
        public int postId { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public string user_Id { get; set; }
        public string smUserName { get; set; }
    }
}
