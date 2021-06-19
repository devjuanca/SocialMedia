using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.Models
{
    public class Metadata
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemsCount { get; set; }
        public int PageSize { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
    }



   


}
