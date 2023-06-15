using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Common.Behaviours
{
    public class ValidationBehavour<TRequest, TRespone> : IPipelineBehavior<TRequest, TRespone> where TRequest : IRequest<TRespone>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TRespone> Handle(TRequest request, RequestHandlerDelegate<TRespone> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var validatorResults = await Task.WhenAll(_validators.Select(x => x.ValidateAsync(context, cancellationToken)));

            var failures = validatorResults.Where(x => x.Errors.Any())
                                           .SelectMany(x => x.Errors)
                                           .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return await next();
        }


    }
}
