using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.UI.ViewModels
{
   public class UserDetailsViewModel
    {
        public string Name { get; set; }

        public string Lastname { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Phone { get; set; }
        public string PhotoRoute { get; set; }
        public string CountryName { get; set; }
        public string UserName { get; set; }
        
        public int CantPost { get; set; }
        public int CantComment { get; set; }
    }
}
