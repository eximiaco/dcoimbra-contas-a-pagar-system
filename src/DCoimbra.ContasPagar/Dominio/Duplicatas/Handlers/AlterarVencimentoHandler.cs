using CSharpFunctionalExtensions;
using DCoimbra.ContasPagar.Dominio.Duplicatas.Comandos;
using MediatR;

namespace DCoimbra.ContasPagar.Dominio.Duplicatas.Handlers;

public sealed class AlterarVencimentoHandler : IRequestHandler<AlterarVencimentoCommand, Result>
{
    public async Task<Result> Handle(AlterarVencimentoCommand request, CancellationToken cancellationToken)
    {
        // Recuperar duplicata via repositorio
        
        // Chamar dominio para alterar vencimento
        
        // Salvar alteracoes
        
        return Result.Success();
    }
}