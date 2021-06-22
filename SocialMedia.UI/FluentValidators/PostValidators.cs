using FluentValidation;
using SocialMedia.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI.FluentValidators
{
    public class PostValidators : AbstractValidator<PostCommandViewModel>
    {
        public PostValidators()
        {
            RuleFor(a => a.Description).NotNull().WithMessage("Must write a post.");
        }
    }
}
