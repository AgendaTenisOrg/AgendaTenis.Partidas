using AgendaTenis.Notificacoes.Core;
using AgendaTenis.Notificacoes.Core.Enums;
using AgendaTenis.Partidas.Core.Dominio;
using AgendaTenis.Partidas.Core.Enums;
using AgendaTenis.Partidas.Core.Repositorios;

namespace AgendaTenis.Partidas.Core.Aplicacao.RegistrarPlacar;

public class RegistrarPlacarHandler
{
    private readonly IPartidasRepositorio _partidaRepositorio;

    public RegistrarPlacarHandler(IPartidasRepositorio partidaRepositorio)
    {
        _partidaRepositorio = partidaRepositorio;
    }

    public async Task<RegistrarPlacarResponse> Handle(RegistrarPlacarCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var partida = await _partidaRepositorio.ObterPorIdAsync(request.Id.ToString());

            var notificacoes = ExecutarValidacoes(partida);
            if (notificacoes.Any(c => c.Tipo == TipoNotificacaoEnum.Erro || c.Tipo == TipoNotificacaoEnum.Aviso))
            {
                return new RegistrarPlacarResponse()
                {
                    Sucesso = false,
                    Notificacoes = notificacoes
                };
            }

            var sets = request.Sets.Select(c => new Set(c.NumeroSet, c.GamesDesafiante, c.GamesAdversario, c.TiebreakDesafiante, c.TiebreakAdversario)).ToList();
            partida.RegistrarPlacar(request.VencedorId, sets);

            var atualizou = await _partidaRepositorio.Update(partida);

            if (atualizou)
            {
                return new RegistrarPlacarResponse()
                {
                    Sucesso = true
                };
            }
            else
            {
                return new RegistrarPlacarResponse()
                {
                    Sucesso = false,
                    Notificacoes = new List<Notificacao>()
                    {
                        new Notificacao()
                        {
                            Mensagem = "Partida não foi atualizada",
                            Tipo = TipoNotificacaoEnum.Aviso
                        }
                    }
                };
            }
        }
        catch (Exception)
        {
            return new RegistrarPlacarResponse()
            {
                Sucesso = false,
                Notificacoes = new List<Notificacao>()
                    {
                        new Notificacao()
                        {
                            Mensagem = "Erro ao atualizar partida",
                            Tipo = TipoNotificacaoEnum.Erro
                        }
                    }
            };
        }
    }

    private List<Notificacao> ExecutarValidacoes(Partida partida)
    {
        var notificacoes = new List<Notificacao>();

        if (partida is null)
        {
            notificacoes.Add(new Notificacao()
            {
                Mensagem = "Partida não encontrada",
                Tipo = TipoNotificacaoEnum.Aviso
            });

            return notificacoes;
        }

        if (partida.StatusConvite != StatusConviteEnum.Aceito)
            notificacoes.Add(new Notificacao()
            {
                Mensagem = "Não é possível atualizar o placar, pois o convite para a partida não foi aceito",
                Tipo = TipoNotificacaoEnum.Aviso
            });

        if (DateTime.UtcNow < partida.DataDaPartida.ToUniversalTime())
            notificacoes.Add(new Notificacao()
            {
                Mensagem = "Não é possível atualizar o placar, pois a partida ainda não aconteceu",
                Tipo = TipoNotificacaoEnum.Aviso
            });

        if (partida.StatusPlacar != null)
            notificacoes.Add(new Notificacao()
            {
                Mensagem = "Placar já foi registrado.",
                Tipo = TipoNotificacaoEnum.Aviso
            });

        return notificacoes;
    }
}
