using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.DTO
{

    public class DataToken
    {
        public string Token { get; set; }
        public string Id { get; set; }
        public List<string> Roles { get; set; }
    }

    public class TokenResult
    {
        public DataToken data { get; set; }
    }
}


