using FluentValidation;
using SocialMedia.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SocialMedia.Application.FluentValidators
{
   public class LoginModelValidation : AbstractValidator<LoginModel>
    {
    

        public LoginModelValidation()
        {
            RuleFor(a => a.UserName).NotEmpty().WithMessage("You must write your username.");
            RuleFor(a => a.Password).NotEmpty().WithMessage("You must write your password.");
        }
    }
}
