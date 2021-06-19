using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SocialMedia.Domain.Entities
{
    public partial class SMUser : IdentityUser
    {
        public SMUser()
        {
            Comment = new HashSet<Comment>();
            Post = new HashSet<Post>();
        }

        public string Name { get; set; }
        public string Lastname { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool? Active { get; set; }
        public string ProfilePhoto { get; set; }

        public int? CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Post> Post { get; set; }
    }
}
