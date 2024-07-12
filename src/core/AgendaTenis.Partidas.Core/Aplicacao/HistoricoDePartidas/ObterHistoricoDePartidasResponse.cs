using AgendaTenis.Partidas.Core.Enums;

namespace AgendaTenis.Partidas.Core.Aplicacao.HistoricoDePartidas;

public class ObterHistoricoDePartidasResponse
{
    public IEnumerable<Partida> Partidas { get; set; }
    public int TotalDeItens { get; set; }

    public class Partida
    {
        public string Id { get; set; }
        public int DesafianteId { get; set; }
        public string DesafianteNome { get; set; }
        public int AdversarioId { get; set; }
        public string AdversarioNome { get; set; }
        public DateTime DataDaPartida { get; set; }
        public int IdCidade { get; set; }
        public string NomeCidade { get; set; }
        public ModeloPartidaEnumModel ModeloDaPartida { get; set; }
        public StatusConviteEnumModel StatusConvite { get; set; }
        public StatusPlacarEnumModel? StatusPlacar { get; set; }
        public int? VencedorId { get; set; }
        public IEnumerable<Set> Sets { get; set; }
    }

    public class Set
    {
        public int NumeroSet { get; set; }
        public int GamesDesafiante { get; set; }
        public int GamesAdversario { get; set; }
        public int? TiebreakDesafiante { get; set; }
        public int? TiebreakAdversario { get; set; }
    }
}
