using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs
{
    public class PaginatedDtoValidator : AbstractValidator<PaginatedDto>
    {
        public PaginatedDtoValidator()
        {
            RuleFor(p => p.Page)
                .Must(p => p > 0)
                .WithMessage(ValidationMessages.PropertyHasInvalidValue(nameof(PaginatedDto.Page)));
            RuleFor(p => p.PageSize)
                .Must(p => p > 0)
                .WithMessage(ValidationMessages.PropertyHasInvalidValue(nameof(PaginatedDto.PageSize)));
        }
    }
}
