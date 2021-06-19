using System;
using System.Collections.Generic;

namespace SocialMedia.Domain.Entities
{
    public partial class Post
    {
        public Post()
        {
            Comment = new HashSet<Comment>();
        }

        public int PostId { get; set; }
        
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string SmuserId { get; set; }
        public virtual SMUser Smuser { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
    }
}
