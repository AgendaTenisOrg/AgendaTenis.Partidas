namespace AgendaTenis.Partidas.Core.Enums;

public enum ModeloPartidaEnum
{
    SetUnico = 1,
    MelhorDeTresSets = 2,
    MelhorDeCincoSets = 3
}

public class ModeloPartidaEnumModel : BaseEnumModel<ModeloPartidaEnum>
{
    public ModeloPartidaEnumModel(ModeloPartidaEnum id) : base(id)
    {
    }

    protected override string ObterDescricao()
    {
        return Id switch
        {
            ModeloPartidaEnum.SetUnico => "Set único",
            ModeloPartidaEnum.MelhorDeTresSets => "Melhor de três sets",
            ModeloPartidaEnum.MelhorDeCincoSets => "Melhor de cinco sets",
            _ => throw new ArgumentOutOfRangeException(nameof(Id), Id, null)
        };
    }
}