namespace AgendaTenis.Partidas.Core.Aplicacao.HistoricoDePartidas;

public class ObterHistoricoDePartidasCommand
{
    public int UsuarioId { get; set; }
    public int Pagina { get; set; }
    public int ItensPorPagina { get; set; }
}
