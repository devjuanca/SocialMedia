using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialMedia.Application.ExceptionsResult;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SocialMedia.Application.CustomExceptions
{
   public class AuthenticationException : ExceptionAbstract
    {
        public AuthenticationException(string message, Dictionary<string, string[]> errors) : base(message)
        {
            Errors = errors;
        }

        public override void BuidException(ExceptionContext context, string type)
        {
            var exception = (context.Exception as AuthenticationException);

            var json = new GenericExceptionResult
            {
                Type = type,
                Title = Message,
                Status = (int)HttpStatusCode.Unauthorized,
                TraceId = this.HResult.ToString(),
                Errors = Errors

            };
            context.Result = new BadRequestObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.ExceptionHandled = true;

        }
    }
}
