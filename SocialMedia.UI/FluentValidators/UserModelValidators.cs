using FluentValidation;
using SocialMedia.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.FluentValidators
{
    public class UserModelValidators : AbstractValidator<UserModel>
    {
        public UserModelValidators()
        {
            RuleFor(a => a.Name)
                .NotNull()
                .WithMessage("Must write your name.");
            RuleFor(a => a.Lastname)
                .NotEmpty()
                .WithMessage("Must write your last name.");
            RuleFor(a => a.BirthDate)
                .NotEmpty()
                .WithMessage("Must write your birth date.");
            RuleFor(a => a.Email)
                .NotEmpty()
                .WithMessage("Must write your email address.")
                .EmailAddress()
                .WithMessage("Must write a valid email address.");
            RuleFor(a => a.Password)
                .NotEmpty()
                .WithMessage("Must write a password.");
            RuleFor(a => a.ConfirmPassword)
                .Equal(a => a.Password)
                .WithMessage("Passwords doesn't match.");
            RuleFor(a => a.UserName)
                .NotEmpty()
                .WithMessage("Must write an user name");
            RuleFor(a => a.CountryId).NotEmpty().WithMessage("Must choose a country.");

                

                
        }
    }
}
