using AgendaTenis.Partidas.Core.Servicos;

namespace AgendaTenis.Partidas.WebApi.ConfiguracaoDeServicos;

public static class CidadeServicoDI
{
    public static void AdicionarCidadeServico(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ICidadeServico, CidadeServico>(
            client =>
            {
                var config = configuration.GetSection("Servicos:Cidades").Get<CidadeServicoConfiguracao>();

                client.BaseAddress = new Uri(config.Url);

                client.DefaultRequestHeaders.Add("Chave", config.Chave);
            });
    }
}

public class CidadeServicoConfiguracao
{
    public string Url { get; set; }
    public string Chave { get; set; }
}
