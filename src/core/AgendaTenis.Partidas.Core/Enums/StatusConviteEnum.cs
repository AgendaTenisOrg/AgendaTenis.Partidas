namespace AgendaTenis.Partidas.Core.Enums;

public enum StatusConviteEnum
{
    Pendente = 1,
    Aceito = 2,
    Recusado = 3
}

public class StatusConviteEnumModel : BaseEnumModel<StatusConviteEnum>
{
    public StatusConviteEnumModel(StatusConviteEnum id) : base(id)
    {
    }

    protected override string ObterDescricao()
    {
        return Id switch
        {
            StatusConviteEnum.Pendente => "Pendente",
            StatusConviteEnum.Aceito => "Aceito",
            StatusConviteEnum.Recusado => "Recusado",
            _ => throw new ArgumentOutOfRangeException(nameof(Id), Id, null)
        };
    }
}