using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs.Authors
{
    public class CreateAuthorValidator : AbstractValidator<CreateAuthorDto>
    {
        public CreateAuthorValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .NotNull()
                .WithMessage(ValidationMessages.PropertyMustNotBeEmpty(nameof(CreateAuthorDto.FirstName)))
                .Length(1, 50)
                .WithMessage(ValidationMessages.PropertyHasInvalidLenght(nameof(CreateAuthorDto.FirstName)));

            RuleFor(x => x.LastName)
                   .NotEmpty()
                   .NotNull()
                   .WithMessage(ValidationMessages.PropertyIsRequired(nameof(CreateAuthorDto.LastName)))
                   .Length(1, 50)
                   .WithMessage(ValidationMessages.PropertyHasInvalidLenght(nameof(CreateAuthorDto.LastName)));
        }
    }
}
