using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Application.Entities
{
   public class DataToken
    {
        public string Token { get; set; }
        public string Id { get; set; }
        public List<string> Roles { get; set; }
    }
}
