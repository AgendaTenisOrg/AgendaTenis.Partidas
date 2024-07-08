using AgendaTenis.Eventos.Configuracao;
using AgendaTenis.Eventos.Servicos;

namespace AgendaTenis.WebApi.ConfiguracaoDeServicos;

public static class MessageBusDI
{
    public static void RegistrarMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMQConfiguracao>(configuration.GetSection("RabbitMQ"));
        services.AddSingleton<IMessageBus, RabbitMessageBus>();
    }
}
