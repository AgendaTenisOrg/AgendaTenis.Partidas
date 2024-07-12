namespace AgendaTenis.Partidas.Core.Enums;

public enum StatusPlacarEnum
{
    AguardandoConfirmacao = 1,
    Aceito = 2,
    Contestado = 3
}

public class StatusPlacarEnumModel : BaseEnumModel<StatusPlacarEnum>
{
    public StatusPlacarEnumModel(StatusPlacarEnum id) : base(id)
    {
    }

    protected override string ObterDescricao()
    {
        return Id switch
        {
            StatusPlacarEnum.AguardandoConfirmacao => "Aguardando confirmação",
            StatusPlacarEnum.Aceito => "Aceito",
            StatusPlacarEnum.Contestado => "Contestado",
            _ => ""
        };
    }
}
