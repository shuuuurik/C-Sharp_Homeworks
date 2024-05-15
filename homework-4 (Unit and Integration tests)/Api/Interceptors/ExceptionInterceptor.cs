using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Api.Interceptors;

public class ExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, e.Message));
        }
        // Не понял, какого рода обработка должна быть в ExceptionInterceptor
    }
}