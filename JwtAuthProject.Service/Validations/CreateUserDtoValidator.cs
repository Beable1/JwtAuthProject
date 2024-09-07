using FluentValidation;
using JwtAuthProject.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthProject.Service.Validations
{
    public class CreateUserDtoValidator:AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("{propertyName} is required").EmailAddress().WithMessage("Email is wrong");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("{propertyName} is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("{propertyName} is required");

        }
    }
}
