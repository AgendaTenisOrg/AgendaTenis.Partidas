using AgendaTenis.Partidas.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AgendaTenis.Partidas.Core.Dominio;

public class Partida
{
    public Partida(int desafianteId, int adversarioId, DateTime dataDaPartida, string descricaoLocal, ModeloPartidaEnum modeloDaPartida)
    {
        DesafianteId = desafianteId;
        AdversarioId = adversarioId;
        DataDaPartida = dataDaPartida;
        DescricaoLocal = descricaoLocal;
        ModeloDaPartida = modeloDaPartida;
        StatusConvite = StatusConviteEnum.Pendente;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int DesafianteId { get; private set; }
    public int AdversarioId { get; private set; }
    public DateTime DataDaPartida { get; private set; }
    public string DescricaoLocal { get; private set; }
    public ModeloPartidaEnum ModeloDaPartida { get; private set; }
    public StatusConviteEnum StatusConvite { get; private set; }
    public StatusPlacarEnum? StatusPlacar { get; private set; }
    public int? VencedorId { get; private set; }
    public List<Set> Sets { get; private set; }

    public void RegistrarPlacar(int vencedorId, List<Set> sets)
    {
        VencedorId = vencedorId;
        Sets = sets;
        StatusPlacar = StatusPlacarEnum.AguardandoConfirmacao;
    }

    public void ResponderPlacar(StatusPlacarEnum statusPlacar)
    {
        StatusPlacar = statusPlacar;
    }

    public void ResponderConvite(StatusConviteEnum statusConvite)
    {
        StatusConvite = statusConvite;
    }
}

public class Set
{
    public Set(int numeroSet, int gamesDesafiante, int gamesAdversario, int? tiebreakDesafiante, int? tiebreakAdversario)
    {
        NumeroSet = numeroSet;
        GamesDesafiante = gamesDesafiante;
        GamesAdversario = gamesAdversario;
        TiebreakDesafiante = tiebreakDesafiante;
        TiebreakAdversario = tiebreakAdversario;
    }

    public int NumeroSet { get; set; }
    public int GamesDesafiante { get; set; }
    public int GamesAdversario { get; set; }
    public int? TiebreakDesafiante { get; set; }
    public int? TiebreakAdversario { get; set; }
}
