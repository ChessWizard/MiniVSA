using MediatR;
using System.Diagnostics;
using System.Text.Json;

namespace MiniVSA.CatalogService.Application.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        private const int MAX_PERFORMANCE_SECONDS = 5;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={Request}",
                typeof(TRequest).Name, typeof(TResponse).Name, JsonSerializer.Serialize(request));

            Stopwatch timer = new();
            timer.Start();

            var response = await next();

            timer.Stop();
            var timeTaken = timer.Elapsed;
            if(timeTaken.Seconds > MAX_PERFORMANCE_SECONDS)
            {
                logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds",
                    typeof(TRequest).Name, timeTaken.Seconds);
            }

            logger.LogInformation("[END] Handle request={Request} - Response={Response} - ResponseData={Response}",
                typeof(TRequest).Name, typeof(TResponse).Name, JsonSerializer.Serialize(response));

            return response;
        }
    }
}
