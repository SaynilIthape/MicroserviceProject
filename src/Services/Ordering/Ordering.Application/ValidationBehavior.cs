using FluentValidation;
using MediatR;

namespace Ordering.Application
{
    internal class ValidationBehavior<TRequest, TResponse> : MediatR.IPipelineBehavior<TRequest, TResponse>
            where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // 1. Create validation context
            var context = new ValidationContext<TRequest>(request);

            // 2. Run all validators registered for this request type
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // 3. Gather any failures
            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            // 4. If there are errors, throw a validation exception (or return a result)
            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            // 5. If everything is fine, move to the next step in the pipeline (the Handler)
            return await next(cancellationToken);
        }
    }
}