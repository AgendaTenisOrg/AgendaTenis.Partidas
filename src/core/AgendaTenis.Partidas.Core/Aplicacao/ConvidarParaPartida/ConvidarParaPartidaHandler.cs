using AgendaTenis.Partidas.Core.Repositorios;

namespace AgendaTenis.Partidas.Core.Aplicacao.ConvidarParaPartida;

public class ConvidarParaPartidaHandler
{
    private readonly IPartidasRepositorio _partidaRepositorio;

    public ConvidarParaPartidaHandler(IPartidasRepositorio partidaRepositorio)
    {
        _partidaRepositorio = partidaRepositorio;
    }

    public async Task<ConvidarParaPartidaResponse> Handle(ConvidarParaPartidaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var partida = new AgendaTenis.Partidas.Core.Dominio.Partida(
            request.DesafianteId,
            request.AdversarioId,
            request.DataDaPartida,
            request.DescricaoLocal,
            request.ModeloDaPartida);

            await _partidaRepositorio.InsertAsync(partida);

            return new ConvidarParaPartidaResponse()
            {
                Sucesso = true
            };
        }
        catch (Exception)
        {
            return new ConvidarParaPartidaResponse()
            {
                Sucesso = false,
                Notificacoes = new List<Notificacoes.Notificacao>()
                {
                    new Notificacoes.Notificacao()
                    {
                        Mensagem = "Erro ao gravar partida",
                        Tipo = Notificacoes.Enums.TipoNotificacaoEnum.Erro
                    }
                }
            };
        }

    }
}
