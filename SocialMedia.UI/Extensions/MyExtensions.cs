using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.Extensions
{
    public static class MyExtensions
    {
        public static string MyToString(this Dictionary<string, string[]> str)
        {
            string result = string.Empty;
            foreach (var item in str)
            {
                result += item.Key+ " : <br />";
                foreach (string value in item.Value)
                {
                    result += value + "<br />";
                }

            }
            return result;
        }
    }
}
