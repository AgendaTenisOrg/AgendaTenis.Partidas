using AgendaTenis.Eventos.Base;
using AgendaTenis.Eventos.Constantes;
using AgendaTenis.Eventos.Mensagens;
using AgendaTenis.Eventos.Servicos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AgendaTenis.Partidas.Core.Eventos.Publishers;

public class PlacarConfirmadoPublisher : IEventPublisher<PlacarConfirmadoMensagem>
{
    private const string EXCHANGENAME = ExchangeConstantes.PartidaExchange;

    private readonly IMessageBus _messageBus;

    public PlacarConfirmadoPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public void Publish(PlacarConfirmadoMensagem eventMessage)
    {
        using (var connection = _messageBus.GetConnection())
        using (var channel = _messageBus.GetChannel(connection))
        {
            channel.ExchangeDeclare(exchange: EXCHANGENAME, type: ExchangeType.Fanout, durable: true, autoDelete: false);

            var jsonMessage = JsonSerializer.Serialize(eventMessage);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: EXCHANGENAME, routingKey: string.Empty, basicProperties: properties, body: body);
        }
    }
}
