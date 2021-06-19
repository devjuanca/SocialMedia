using FluentValidation;
using SocialMedia.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Application.FluentValidators
{
    public class PostValidator : AbstractValidator<PostCommandDTO>
    {
        public PostValidator()
        {
            RuleFor(post => post.Description)
                .NotEmpty().WithName("Post Description").WithMessage("You must write something in your post!")
                .Length(10, 500).WithMessage("You must write between 10 and 500 characters!");
            RuleFor(post => post.Image)
                .Must(EsImagen).WithMessage("Wrong format file. It must be an image.");
            
        }

        bool EsImagen(string imagen)
        {
            return imagen != null ? imagen.EndsWith(".jpg") || imagen.EndsWith(".png"): true;
        }
    }

}
