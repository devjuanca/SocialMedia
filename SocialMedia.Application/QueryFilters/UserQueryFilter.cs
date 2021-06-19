using SocialMedia.Application.Enumerations;
using SocialMedia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Application.QueryFilters
{
   public class SMUserQueryFilter : IPagingFilter
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? CountryId { get; set; }
        public SMUserOrderProperties? OrderProperty { get; set; }
        public OrderDirection? OrderDirection { get; set; }

        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
       
    }
}
