using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SocialMedia.Application.CustomExceptions;
using SocialMedia.Application.ExceptionsResult;
using System;
using System.Net;

namespace SocialMedia.Application.Filters.ExceptionFilters
{
    public class GlobalExeptionFilter : IExceptionFilter
    {
        ILogger<GlobalExeptionFilter> _logger;

        public GlobalExeptionFilter(ILogger<GlobalExeptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(BussinessException))
            {
                (context.Exception as BussinessException).BuidException(context, "Bussiness Exception");
            }
            else if (context.Exception.GetType() == typeof(ApiExceptions))
            {
                (context.Exception as ApiExceptions).BuidException(context, "Api Exception");
            }
            else if (context.Exception.GetType() == typeof(NotFoundException))
            {
                (context.Exception as NotFoundException).BuidException(context, "Not Found Exception");
            }
            else if (context.Exception.GetType() == typeof(AuthenticationException))
            {
                (context.Exception as AuthenticationException).BuidException(context, "Authentication Exception");
            }
            else if (context.Exception.GetType() == typeof(ImageUploadException))
            {
                (context.Exception as ImageUploadException).BuidException(context, "Image Upload Exception");
            }
        }

       
    }
}
