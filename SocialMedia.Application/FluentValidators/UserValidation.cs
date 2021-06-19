using FluentValidation;
using SocialMedia.Application.DTOs;
using SocialMedia.Domain.Entities;
using SocialMedia.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SocialMedia.Application.FluentValidators
{
   public class SMUserValidation : AbstractValidator<SMUserComandDTO>
    {
    

        public SMUserValidation()
        {
            RuleFor(SMUser => SMUser.Name).NotEmpty().WithName("SMUser Name").WithMessage("You must write your name.");
            
            RuleFor(SMUser => SMUser.Lastname).NotEmpty().WithMessage("You must write your last name");

            RuleFor(SMUser => SMUser.BirthDate).NotEmpty().WithMessage("You must write your date birth.");

            RuleFor(SMUser => SMUser.Email).NotEmpty().WithMessage("You must write your email").EmailAddress().WithMessage("Wrong email address.");
            
            RuleFor(SMUser => SMUser.Password).Equal(SMUser => SMUser.ConfirmPassword).WithMessage("The password doesn't match.");
                

        }
    }
}
