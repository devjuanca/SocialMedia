using FluentValidation;
using SocialMedia.Application.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Application.FluentValidators
{
   public class ChangePasswordValidators: AbstractValidator<ChangePasswordEntity>
    {
        public ChangePasswordValidators()
        {
            RuleFor(a => a.NewPasword)
                .Equal(a => a.ConfirmNewPassword).WithMessage("Passwords do not match.");
        }
    }
}
