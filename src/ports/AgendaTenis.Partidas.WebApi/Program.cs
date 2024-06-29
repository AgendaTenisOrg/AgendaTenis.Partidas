
using AgendaTenis.Partidas.Core.Aplicacao.ConfirmacoesDePlacarPendentes;
using AgendaTenis.Partidas.Core.Aplicacao.ConvidarParaPartida;
using AgendaTenis.Partidas.Core.Aplicacao.ConvitesPendentes;
using AgendaTenis.Partidas.Core.Aplicacao.HistoricoDePartidas;
using AgendaTenis.Partidas.Core.Aplicacao.RegistrarPlacar;
using AgendaTenis.Partidas.Core.Aplicacao.ResponderConvite;
using AgendaTenis.Partidas.Core.Aplicacao.ResponderPlacar;
using AgendaTenis.Partidas.Core.Eventos.Publishers;
using AgendaTenis.Partidas.Core.Repositorios;
using AgendaTenis.Partidas.WebApi.ConfiguracaoDeServicos;
using AgendaTenis.Partidas.WebApi.Polices;
using AgendaTenis.WebApi.ConfiguracaoDeServicos;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AgendaTenis.Partidas.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IMongoClient>(c =>
            {
                return new MongoClient(builder.Configuration.GetConnectionString("Partidas"));
            });

            builder.Services.AdicionarConfiguracaoSwagger();

            builder.Services.AddScoped<IPartidasRepositorio, PartidasRepositorio>();

            builder.Services.AddScoped<PlacarConfirmadoPublisher>();

            builder.Services.AddScoped<ObterHistoricoDePartidasHandler>();
            builder.Services.AddScoped<ConvidarParaPartidaHandler>();
            builder.Services.AddScoped<ObterConvitesPendentesHandler>();
            builder.Services.AddScoped<ResponderConviteHandler>();
            builder.Services.AddScoped<ObterConfirmacoesDePlacarPendentesHandler>();
            builder.Services.AddScoped<RegistrarPlacarHandler>();
            builder.Services.AddScoped<ResponderPlacarHandler>();

            builder.Services.AddScoped<AdversarioDaPartidaPoliceHandler>();
            builder.Services.AddScoped<DesafianteDaPartidaPoliceHandler>();

            builder.Services.RegistrarMessageBus(builder.Configuration);

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
                options.InstanceName = "AgendaTenis.Partidas";
            });

            builder.Services.AdicionarAutenticacaoJWT(builder.Configuration);

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
