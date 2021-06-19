using SocialMedia.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.ViewModels
{
    public class UserListViewModel
    {
        public UserModel[] Data { get; set; }
        public Metadata Meta { get; set; }
    }
}
