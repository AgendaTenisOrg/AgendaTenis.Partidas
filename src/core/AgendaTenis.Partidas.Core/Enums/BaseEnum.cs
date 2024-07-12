namespace AgendaTenis.Partidas.Core.Enums;

public abstract class BaseEnumModel<TEnum> where TEnum : Enum
{
    protected BaseEnumModel(TEnum id)
    {
        Id = id;
        Descricao = ObterDescricao();
    }

    public TEnum Id { get; set; }
    public string Descricao { get; set; }

    protected abstract string ObterDescricao();
}
