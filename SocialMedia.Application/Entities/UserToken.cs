using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Application.Entities
{
   public class UserToken
    {
        public string UserId{ get; set; }
        public string Token { get; set; }

        public string ReturnUrl { get; set; }
    }
}
