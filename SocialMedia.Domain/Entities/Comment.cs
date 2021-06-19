using System;
using System.Collections.Generic;

namespace SocialMedia.Domain.Entities
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public string SmuserId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public bool? Active { get; set; }

        public virtual Post Post { get; set; }
        public virtual SMUser Smuser { get; set; }
    }
}
