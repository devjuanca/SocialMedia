using System;
using System.Collections.Generic;

namespace SocialMedia.Domain.Entities
{
    public partial class Country
    {
        public Country()
        {
            AspNetUsers = new HashSet<SMUser>();
        }

        public int CountryId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SMUser> AspNetUsers { get; set; }
    }
}
