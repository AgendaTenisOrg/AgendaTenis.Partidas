namespace AgendaTenis.Partidas.Core.Aplicacao.RegistrarPlacar;

public class RegistrarPlacarCommand
{
    public string Id { get; set; }
    public int VencedorId { get; set; }
    public List<Set> Sets { get; set; }

    public class Set
    {
        public int NumeroSet { get; set; }
        public int GamesDesafiante { get; set; }
        public int GamesAdversario { get; set; }
        public int? TiebreakDesafiante { get; set; }
        public int? TiebreakAdversario { get; set; }
    }
}
