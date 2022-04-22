using System;
using FluentValidation;
using SocialMedia.Domain.DTOs;

namespace SocialMedia.Infrastructure.Validators
{
    public class PostValidator: AbstractValidator<PostDTO>
    {
        public PostValidator()
        {
            RuleFor(post => post.Description)
                .NotNull();
                
            RuleFor(post => post.Date)
                .NotNull();
        }
    }
}