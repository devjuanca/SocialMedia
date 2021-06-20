using SocialMedia.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.ViewModels
{
    public class PostListViewModel
    {
        public List<PostModel> data { get; set; }
        
        public Metadata meta { get; set; }
    }
}
