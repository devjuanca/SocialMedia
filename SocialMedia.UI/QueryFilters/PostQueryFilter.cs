using SocialMedia.Application.Enumerations;
using System;
using System.Globalization;

namespace SocialMedia.UI.QueryFilters
{
    public class PostQueryFilter
    {
        public string Id { get; set; }
        public string DescriptionSearch { get; set; }
        public string UserId { get; set; }
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
                {
                    var s = item.PropertyType;
                    if (item.Name == "Date")
                    {
                      string date = ((DateTime)item.GetValue(this)).ToString("d", CultureInfo.InvariantCulture);
                      result += $"{item.Name}={date}&";
                    }
                    else
                      result += $"{item.Name}={item.GetValue(this)}&";
                }
            }
            return result.Substring(0, result.Length - 1);
        }


    }
}
