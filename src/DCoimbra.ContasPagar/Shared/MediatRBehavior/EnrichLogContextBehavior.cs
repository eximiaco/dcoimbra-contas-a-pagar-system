using MediatR;
using Serilog;
using Serilog.Context;

namespace DCoimbra.Shared.MediatRBehavior;

public class EnrichLogContextBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger;

    public EnrichLogContextBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        TResponse resposta;
        if (request.GetType().GetInterfaces().All(c => c != typeof(IDomainCommand)))
            return await next();

        var requisicao = (IDomainCommand)request;
        using (LogContext.PushProperty(LogsContexts.CommandId, requisicao.Id))
        using (LogContext.PushProperty(LogsContexts.CommandTimestamp, requisicao.Timestamp))
        {
            _logger
                .ForContext(LogsContexts.CommandType, requisicao.GetType())
                .Debug("Comando pronto para processamento");
            resposta = await next();
        }

        return resposta;
    }
}