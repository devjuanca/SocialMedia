using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Application.QueryFilters
{
    public interface IPagingFilter
    {
        int? PageNumber { get; set; }
        int? PageSize { get; set; }

    }
}
