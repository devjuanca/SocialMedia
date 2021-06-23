using SocialMedia.Application.Enumerations;
using System;

namespace SocialMedia.Application.QueryFilters
{
    public class PostQueryFilter:IPagingFilter
    {
        public string Id { get; set; }
        public string DescriptionSearch { get; set; }
        public string UserId { get; set; }

        public DateTime? Date { get; set; }
        public PostOrderProperties? OrderProperty { get; set; }
        public OrderDirection? OrderDirection { get; set; }

        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        
        
    }
}
