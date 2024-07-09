using AgendaTenis.Notificacoes.Core;
using AgendaTenis.Notificacoes.Core.Enums;
using AgendaTenis.Partidas.Core.Repositorios;
using AgendaTenis.Partidas.Core.Servicos;

namespace AgendaTenis.Partidas.Core.Aplicacao.ConvidarParaPartida;

public class ConvidarParaPartidaHandler
{
    private readonly IPartidasRepositorio _partidaRepositorio;
    private readonly ICidadeServico _cidadeServico;

    public ConvidarParaPartidaHandler(
        IPartidasRepositorio partidaRepositorio, 
        ICidadeServico cidadeServico)
    {
        _partidaRepositorio = partidaRepositorio;
        _cidadeServico = cidadeServico;
    }

    public async Task<ConvidarParaPartidaResponse> Handle(ConvidarParaPartidaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var cidade = await _cidadeServico.ObterPorId(request.IdCidade);

            if (cidade is null)
            {
                return new ConvidarParaPartidaResponse()
                {
                    Sucesso = false,
                    Notificacoes = new List<Notificacao>()
                    {
                        new Notificacao()
                        {
                            Mensagem = "Cidade não encontrada",
                            Tipo = TipoNotificacaoEnum.Erro
                        }
                    }
                };
            }

            var partida = new AgendaTenis.Partidas.Core.Dominio.Partida(request.DesafianteId,
                                                                        request.AdversarioId,
                                                                        request.DataDaPartida,
                                                                        request.IdCidade,
                                                                        cidade.Nome,
                                                                        request.ModeloDaPartida);

            await _partidaRepositorio.InsertAsync(partida);

            return new ConvidarParaPartidaResponse()
            {
                Sucesso = true
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

            return new ConvidarParaPartidaResponse()
            {
                Sucesso = false,
                Notificacoes = new List<Notificacao>()
                {
                    new Notificacao()
                    {
                        Mensagem = "Erro ao gravar partida",
                        Tipo = TipoNotificacaoEnum.Erro
                    }
                }
            };
        }

    }
}
