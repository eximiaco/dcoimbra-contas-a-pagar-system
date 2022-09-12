using CSharpFunctionalExtensions;
using DCoimbra.Shared;

namespace DCoimbra.ContasPagar.Dominio.Duplicatas.Comandos;

public class AlterarVencimentoCommand : IDomainCommand<Result>
{
    private AlterarVencimentoCommand(Guid id, DateTime timestamp, DateTime novoVencimento)
    {
        Id = id;
        Timestamp = timestamp;
        NovoVencimento = novoVencimento;
    }

    public Guid Id { get; }
    public DateTime Timestamp { get; }
    public DateTime NovoVencimento { get; }

    public static Result<AlterarVencimentoCommand> Criar(DateTime novoVencimento)
    {
        return 
            novoVencimento <= DateTime.UtcNow 
                ? Result.Failure<AlterarVencimentoCommand>("Novo vencimento inválido") 
                : new AlterarVencimentoCommand(Guid.NewGuid(), DateTime.UtcNow, novoVencimento);
    }
}