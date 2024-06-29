using AgendaTenis.Infra.Eventos.Base;

namespace AgendaTenis.Infra.Eventos.Mensagens;

public class PlacarConfirmadoMensagem : IEventMessage
{
    public string Id { get; set; }
    public int DesafianteId { get; set; }
    public int AdversarioId { get; set; }
    public DateTime DataDaPartida { get; set; }
    public int ModeloDaPartida { get; set; }
    public int? VencedorId { get; set; }
}
