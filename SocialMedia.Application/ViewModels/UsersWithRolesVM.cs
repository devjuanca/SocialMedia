using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialMedia.Application.ViewModels
{
   public class UsersWithRolesVM
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<string> Roles { get; set; }
    }
}
