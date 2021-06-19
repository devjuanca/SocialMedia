using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SocialMedia.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.Models
{

    public class UserVM
    {
       public UserModel SMUser { get; set; }
       public IFormFile Image { get; set; }
       public CountryListViewModel Countries { get; set; }
    }

}
