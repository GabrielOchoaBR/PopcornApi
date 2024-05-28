using FluentValidation;
using MediatR;

namespace Application.Validations
{
    public sealed class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IValidator<TRequest>? validator = validator;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validation = (validator != null) ? await validator.ValidateAsync(request, cancellationToken) : null;

            if (validation != null && !validation.IsValid)
            {
                var errors = validation.Errors.Where(x => x is not null)
                                              .GroupBy(
                                                            x => x.PropertyName, 
                                                            x => x.ErrorMessage,
                                                            (propertyName, errorMessages) => new { Key = propertyName, Values = errorMessages.Distinct().ToArray() }
                                                      ).ToDictionary(x => x.Key, x => x.Values);

                if (errors.Count > 0)
                {
                    throw new Exceptions.ValidationException(errors);
                }
            }

            return await next();
        }
    }
}
