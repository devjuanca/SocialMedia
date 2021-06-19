using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Application.Entities
{
   public class ChangePasswordEntity
    {
       public string UserName { get; set; }
       public string OldPassword { get; set; }
       public string NewPasword { get; set; }
       public string ConfirmNewPassword { get; set; }
    }
}
