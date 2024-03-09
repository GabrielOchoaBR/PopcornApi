using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Logging
{
    [ExcludeFromCodeCoverage]
    public class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
    {
        public ILogger<TRequest> logger { get; } = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var stopWatch = Stopwatch.StartNew();

            logger.LogInformation($"Handling {typeof(TRequest).Name} - {DateTime.Now:G}");
            var response = await next();
            logger.LogInformation($"Handled {typeof(TResponse).Name} - {DateTime.Now:G} ({stopWatch.ElapsedMilliseconds}ms).");

            return response;
        }
    }
}
