using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Sms.Application.Pipelines;

public sealed class ValidatorPipeline<TRequest, TResponse>(IValidator<TRequest> validator, ILogger<ValidatorPipeline<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            logger.LogInformation("Validation failed for {}", typeof(TRequest).Name);
            throw new ValidationException(string.Join(' ', validation.Errors.Select(e => e.ErrorMessage)));
        }

        return await next();
    }
}