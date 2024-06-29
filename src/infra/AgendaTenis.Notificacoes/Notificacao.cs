using AgendaTenis.Notificacoes.Enums;

namespace AgendaTenis.Notificacoes;

public class Notificacao
{
    public string Mensagem { get; set; }
    public TipoNotificacaoEnum Tipo { get; set; }
}
