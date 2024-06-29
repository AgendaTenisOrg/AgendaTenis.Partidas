using AgendaTenis.Infra.Eventos.Configuracao;
using AgendaTenis.Infra.Eventos.Servicos;

namespace AgendaTenis.WebApi.ConfiguracaoDeServicos;

public static class MessageBus
{
    public static void RegistrarMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMQConfiguracao>(configuration.GetSection("RabbitMQ"));
        services.AddSingleton<IMessageBus, RabbitMessageBus>();
    }
}
