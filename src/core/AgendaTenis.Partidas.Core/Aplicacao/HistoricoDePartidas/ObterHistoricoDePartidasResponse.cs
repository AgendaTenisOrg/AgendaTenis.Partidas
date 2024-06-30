using AgendaTenis.Partidas.Core.Enums;

namespace AgendaTenis.Partidas.Core.Aplicacao.HistoricoDePartidas;

public class ObterHistoricoDePartidasResponse
{
    public List<Partida> Partidas { get; set; }

    public class Partida
    {
        public string Id { get; set; }
        public int DesafianteId { get; set; }
        public int AdversarioId { get; set; }
        public DateTime DataDaPartida { get; set; }
        public int IdCidade { get; set; }
        public string NomeCidade { get; set; }
        public ModeloPartidaEnum ModeloDaPartida { get; set; }
        public StatusConviteEnum StatusConvite { get; set; }
        public StatusPlacarEnum? StatusPlacar { get; set; }
        public int? VencedorId { get; set; }
    }
}
