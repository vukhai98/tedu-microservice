﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(p => p.UserName).NotEmpty().WithMessage("{UserName} is required.")
                          .NotNull().MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

            RuleFor(p => p.EmailAddress).EmailAddress().WithMessage("{EmailAddress} is invalid format.")
                          .NotEmpty().WithMessage("{EmailAddress} is required.");

            RuleFor(p => p.TotalPrice).NotEmpty().WithMessage("{TotalPrice} is required.")
                        .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");
        }
    }
}
