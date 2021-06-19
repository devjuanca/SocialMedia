using FluentValidation;
using SocialMedia.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Application.FluentValidators
{
   public class CommentValidator : AbstractValidator<CommentCommandDTO>
    {
        public CommentValidator()
        {
            RuleFor(comment => comment.Description)
                .NotEmpty().WithName("Post Description").WithMessage("You must write something in your comment!")
                .Length(1, 50).WithMessage("You must write between 1 and 50 characters!");
       

        }
    }
}
