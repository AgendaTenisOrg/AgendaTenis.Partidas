using AgendaTenis.Notificacoes.Core;

namespace AgendaTenis.Partidas.Core.Aplicacao.RegistrarPlacar;

public class RegistrarPlacarResponse
{
    public bool Sucesso { get; set; }
    public List<Notificacao> Notificacoes { get; set; }
}
