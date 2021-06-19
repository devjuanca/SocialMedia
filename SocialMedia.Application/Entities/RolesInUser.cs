using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Application.Entities
{
   public class RolesInUser
    {
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
