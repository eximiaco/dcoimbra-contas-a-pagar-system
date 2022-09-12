using DCoimbra.Shared;

namespace DCoimbra.ContasPagar.Dominio.Duplicatas;

public sealed class Duplicata : EntityBase
{
    public DateTime Vencimento { get; private set; }
}