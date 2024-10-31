using FluentValidation;
using MediatR;

namespace MiniVSA.CatalogService.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseRequest
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ValidationContext<TRequest> validationContext = new(request);

            var validationResults = await Task.WhenAll(validators.Select(validator => validator.ValidateAsync(validationContext, cancellationToken)));

            var failures = validationResults.Where(r => r.Errors.Any())
                                            .SelectMany(r => r.Errors)
                                            .ToList();

            if (failures.Any())
                throw new ValidationException(failures);

            var response = await next();
            return response;
        }
    }
}
