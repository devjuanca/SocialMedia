using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.Exceptions
{

    public class CustomApiException: Exception
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }
       public Dictionary<string, string[]> Errors { get; set; }
    }


}
