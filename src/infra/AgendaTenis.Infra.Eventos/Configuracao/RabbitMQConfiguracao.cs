namespace AgendaTenis.Infra.Eventos.Configuracao;

public class RabbitMQConfiguracao
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string CliendProvidedName { get; set; }
    public string VirtualHost { get; set; }

}
