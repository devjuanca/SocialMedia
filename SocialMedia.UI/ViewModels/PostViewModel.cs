using SocialMedia.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.ViewModels
{
    public class PostCommandViewModel
    {
        public string User_Id { get; set; }
        public int PostId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }

    public class PostViewModel
    {
        public int PostId { get; set; }
        public string User_Id { get; set; }
        public string SmUserName { get; set; }
        public string Name { get; set; }
        public string ProfilePhoto { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public List<CommentModel> Comments { get; set; }
    }
}
