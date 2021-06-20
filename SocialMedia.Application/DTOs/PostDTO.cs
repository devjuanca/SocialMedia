using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialMedia.Application.DTOs
{
   public class PostDTO
    {
        public int PostId { get; set; }
        public string User_Id { get; set; }
        public string SMUserName { get; set; }
        public string Name { get; set; }
        public string ProfilePhoto { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
    public class PostCommandDTO
    {
        public string User_Id { get; set; }
        public int PostId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
