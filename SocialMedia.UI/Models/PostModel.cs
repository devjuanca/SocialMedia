using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.Models
{
    public class PostModel
    {
        public int postId { get; set; }
        public string user_Id { get; set; }
        public string smUserName { get; set; }
        public string name { get; set; }
        public string profilePhoto { get; set; }
        public DateTime date { get; set; }
        public string description { get; set; }
        public string image { get; set; }
    }
}
