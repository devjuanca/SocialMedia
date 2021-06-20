using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.Models
{
    public class PostModel
    {
        public int PostId { get; set; }
        public string User_Id { get; set; }
        public string SmUserName { get; set; }
        public string Name { get; set; }
        public string ProfilePhoto { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
