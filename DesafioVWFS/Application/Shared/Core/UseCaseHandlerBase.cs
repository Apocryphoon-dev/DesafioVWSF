using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DesafioVWFS.Application.Shared.Core;

public abstract class UseCaseHandlerBase<TRequest, TResponse> : IUseCase<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    private readonly ILogger<UseCaseHandlerBase<TRequest, TResponse>> _logger;
    private readonly IValidator<TRequest>? _validator;

    protected UseCaseHandlerBase(ILogger<UseCaseHandlerBase<TRequest, TResponse>> logger, IValidator<TRequest>? validator = null)
    {
        _logger = logger;
        _validator = validator;
    }

    public async Task<Output<TResponse>> ExecuteAsync(TRequest request, CancellationToken cancellationToken)
    {
        if (_validator is not null)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var output = new Output<TResponse>(false);
                output.AddErrorMessages(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
                return output;
            }
        }

        try
        {
            return await HandleAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error while processing {RequestName}", typeof(TRequest).Name);
            var output = new Output<TResponse>(false);
            output.AddErrorMessage("Unhandled error while processing request.");
            return output;
        }
    }

    protected abstract Task<Output<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
