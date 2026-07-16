namespace DesafioVWFS.Application.Shared.Core;

public interface IUseCase<in TInput, TOutput> where TInput : notnull, IRequest<TOutput> where TOutput : notnull
{
    Task<Output<TOutput>> ExecuteAsync(TInput request, CancellationToken cancellationToken);
}

public interface IUseCase<in TInput> where TInput : notnull, IRequest
{
    Task<Output> ExecuteAsync(TInput request, CancellationToken cancellationToken);
}
