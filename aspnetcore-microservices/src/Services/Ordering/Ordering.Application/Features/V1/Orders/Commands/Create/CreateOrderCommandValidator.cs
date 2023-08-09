﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Commands.Create
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(p => p.UserName).NotEmpty().WithMessage("{UserName} is required.")
                          .NotNull().MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

        }
    }
}
