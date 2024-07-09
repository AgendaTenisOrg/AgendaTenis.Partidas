using AgendaTenis.Partidas.Core.Aplicacao.ConfirmacoesDePlacarPendentes;
using AgendaTenis.Partidas.Core.Aplicacao.ConvidarParaPartida;
using AgendaTenis.Partidas.Core.Aplicacao.ConvitesPendentes;
using AgendaTenis.Partidas.Core.Aplicacao.HistoricoDePartidas;
using AgendaTenis.Partidas.Core.Aplicacao.RegistrarPlacar;
using AgendaTenis.Partidas.Core.Aplicacao.ResponderConvite;
using AgendaTenis.Partidas.Core.Aplicacao.ResponderPlacar;
using AgendaTenis.Partidas.Core.Eventos.Publishers;
using AgendaTenis.Partidas.Core.Repositorios;
using AgendaTenis.Partidas.Core.Servicos;
using AgendaTenis.Partidas.WebApi.ConfiguracaoDeServicos;
using AgendaTenis.Partidas.WebApi.Polices;
using AgendaTenis.WebApi.ConfiguracaoDeServicos;
using MongoDB.Driver;

namespace AgendaTenis.Partidas.WebApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddScoped<IMongoClient>(c =>
        {
            return new MongoClient(Configuration.GetConnectionString("Partidas"));
        });

        services.AdicionarConfiguracaoSwagger();

        services.AddScoped<IPartidasRepositorio, PartidasRepositorio>();

        services.AddScoped<PlacarConfirmadoPublisher>();

        services.AddScoped<ObterHistoricoDePartidasHandler>();
        services.AddScoped<ConvidarParaPartidaHandler>();
        services.AddScoped<ObterConvitesPendentesHandler>();
        services.AddScoped<ResponderConviteHandler>();
        services.AddScoped<ObterConfirmacoesDePlacarPendentesHandler>();
        services.AddScoped<RegistrarPlacarHandler>();
        services.AddScoped<ResponderPlacarHandler>();

        services.AddScoped<AdversarioDaPartidaPoliceHandler>();
        services.AddScoped<DesafianteDaPartidaPoliceHandler>();

        services.RegistrarMessageBus(Configuration);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Configuration.GetConnectionString("Redis");
            options.InstanceName = "AgendaTenis.Partidas";
        });

        services.AdicionarAutenticacaoJWT(Configuration);

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AdicionarCidadeServico(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.EnvironmentName == "Container")
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
