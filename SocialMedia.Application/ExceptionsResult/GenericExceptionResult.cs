using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Application.ExceptionsResult
{
   public class GenericExceptionResult
    {
        public string Type{ get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }
        public Dictionary<string,string[]> Errors { get; set; }
    }
}
