using DCoimbra.ContasPagar.Dominio.Duplicatas.Comandos;
using DCoimbra.ContasPagar.HttpWebApi.InputModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace DCoimbra.ContasPagar.HttpWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DuplicatasController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public DuplicatasController(
        ILogger logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet()]
    public async Task<IActionResult> GetByFilter()
    {
        // TODO : Chamar infra de consulta e devolver uma view model
        // TODO : Usar conversão implicita para retornar um SuccessResult
        return Ok();
    }

    [HttpPost("{id}/alterar-vencimento")]
    public async Task<IActionResult> AlterarVencimento([FromBody] AlterarVencimentoInputModel inputModel, Guid id,
        CancellationToken cancellationToken)
    {
        var comando = AlterarVencimentoCommand.Criar(inputModel.NovoVencimento);
        if (comando.IsFailure)
        {
            _logger.Warning("Falha ao processar requisição");
            // TODO : Retorar conversão implicita de FailureResult
            return BadRequest(comando.Error);
        }

        var resultado = await _mediator.Send(comando.Value, cancellationToken);
        if(resultado.IsFailure)
        {
            _logger.Warning("Falha ao processar comando");
            // TODO : Retorar conversão implicita de FailureResult
            return BadRequest(resultado.Error);
        }

        // TODO : Retorar conversão implicita de SuccessResult 
        return Ok();
    }
}