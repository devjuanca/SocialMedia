using SocialMedia.Application.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.QueryFilters
{
    public class UserQueryFilter 
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public SMUserOrderProperties? OrderProperty { get; set; }
        public OrderDirection? OrderDirection { get; set; }

        public int? CountryId { get; set; }

        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public override string ToString()
        {
            string result = "?";
            foreach (var item in typeof(UserQueryFilter).GetProperties())
            {
                if(item.GetValue(this)!=null)
                    result += $"{item.Name}={item.GetValue(this)}&";
            }
            return result.Substring(0, result.Length - 1);
        }

    }
}
