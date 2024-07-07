using AgendaTenis.Notificacoes.Core;

namespace AgendaTenis.Partidas.Core.Aplicacao.ConvidarParaPartida;

public class ConvidarParaPartidaResponse
{
    public bool Sucesso { get; set; }
    public List<Notificacao> Notificacoes { get; set; }
}
