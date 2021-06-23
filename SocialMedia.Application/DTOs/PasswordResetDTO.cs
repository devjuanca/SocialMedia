using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Application.DTOs
{
   public class PasswordResetDTO
    { 
        public string Username { get; set; }
        public string ReturnUrl { get; set; }
    }
}
