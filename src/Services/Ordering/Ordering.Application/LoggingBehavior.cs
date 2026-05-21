using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Ordering.Application
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // 1. Log the start of the request
                _logger.LogInformation("[START] Handle request {RequestName} with content {RequestContent}",
                    typeof(TRequest).Name, request);

                var timer = new Stopwatch();
                timer.Start();

                // 2. Continue to the next behavior or handler
                var response = await next().ConfigureAwait(false);

                timer.Stop();

                // 3. Log the completion and performance
                _logger.LogInformation("[END] Handled {RequestName} with {ResponseName} in {Elapsed}ms",
                    typeof(TRequest).Name, typeof(TResponse).Name, timer.ElapsedMilliseconds);

                return response;
        }
    }

}