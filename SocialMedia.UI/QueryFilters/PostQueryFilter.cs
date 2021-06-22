using SocialMedia.Application.Enumerations;
using System;

namespace SocialMedia.UI.QueryFilters
{
    public class PostQueryFilter
    {
        public string Id { get; set; }
        public string DescriptionSearch { get; set; }
        public DateTime? Date { get; set; }
        public PostOrderProperties? OrderProperty { get; set; }
        public OrderDirection? OrderDirection { get; set; }

        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public override string ToString()
        {
            string result = "?";
            foreach (var item in typeof(PostQueryFilter).GetProperties())
            {
                if (item.GetValue(this) != null)
                    result += $"{item.Name}={item.GetValue(this)}&";
            }
            return result.Substring(0, result.Length - 1);
        }


    }
}
