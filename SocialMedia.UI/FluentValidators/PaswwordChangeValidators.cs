using FluentValidation;
using SocialMedia.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.FluentValidators
{
    public class PaswwordChangeValidators: AbstractValidator<ChangePasswordModel>
    {
        public PaswwordChangeValidators()
        {
            RuleFor(a => a.UserName).NotNull().WithMessage("Must write your username.");
            RuleFor(a => a.OldPassword).NotNull().WithMessage("Must write your actual password.");
            RuleFor(a => a.NewPasword).NotNull().WithMessage("Must write your new password.");
            RuleFor(a => a.ConfirmNewPassword)
                .NotNull().WithMessage("Must confirm your new password.")
                .Equal(a => a.NewPasword).WithMessage("The passwords doesn' t match.");
        }
    }
}
