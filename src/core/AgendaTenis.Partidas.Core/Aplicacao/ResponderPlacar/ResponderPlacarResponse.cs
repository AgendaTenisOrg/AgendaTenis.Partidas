using AgendaTenis.Notificacoes.Core;

namespace AgendaTenis.Partidas.Core.Aplicacao.ResponderPlacar;

public class ResponderPlacarResponse
{
    public bool Sucesso { get; set; }
    public List<Notificacao> Notificacoes { get; set; }
}
