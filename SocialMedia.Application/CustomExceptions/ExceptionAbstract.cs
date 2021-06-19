using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SocialMedia.Application.CustomExceptions
{
   public abstract class  ExceptionAbstract : Exception
    {
        public Dictionary<string, string[]> Errors { get; set; }
        

        public ExceptionAbstract(string message) : base(message)
        {}

        public abstract void BuidException(ExceptionContext context, string type);
    }
}
