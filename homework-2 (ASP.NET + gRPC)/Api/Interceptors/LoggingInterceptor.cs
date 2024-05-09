using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Api.Interceptors;

public class LoggingInterceptor : Interceptor
{
    private ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        
        TResponse? response;
        try
        {
            _logger.LogInformation($"Request: {request}");
            response = await continuation(request, context);
            _logger.LogInformation($"Response: {response}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Ooooops, something went wrong {Environment.NewLine}" +
                                $"Error thrown by: {context.Method}.");
            throw;
        }
        return response;
    }
}