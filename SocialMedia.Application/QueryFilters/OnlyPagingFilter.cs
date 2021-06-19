using SocialMedia.Application.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Application.QueryFilters
{
   public class OnlyPagingFilter : IPagingFilter
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

    }
}
