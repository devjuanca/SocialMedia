using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialMedia.Application.ExceptionsResult;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SocialMedia.Application.CustomExceptions
{
   public class ImageUploadException : ExceptionAbstract
    {
      
        public ImageUploadException(string message, Dictionary<string, string[]> errors) : base(message)
        {}

        public override void BuidException(ExceptionContext context, string type)
        {
            var exception = (context.Exception as ImageUploadException);

            var json = new GenericExceptionResult
            {
                Type = type,
                Title = Message,
                Status = (int)HttpStatusCode.BadRequest,
                TraceId = this.HResult.ToString(),
                Errors = Errors

            };
            context.Result = new BadRequestObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.ExceptionHandled = true;

        }
    }
}
