namespace AgendaTenis.Partidas.Core.Enums;

public enum JogadoresEnum
{
    Desafiante = 1,
    Adversario = 2
}

public class JogadoresEnumModel : BaseEnumModel<JogadoresEnum>
{
    public JogadoresEnumModel(JogadoresEnum id) : base(id)
    {
    }

    protected override string ObterDescricao()
    {
        return Id switch
        {
            JogadoresEnum.Desafiante => "Desafiante",
            JogadoresEnum.Adversario => "Adversário",
            _ => throw new ArgumentOutOfRangeException(nameof(Id), Id, null)
        };
    }
}