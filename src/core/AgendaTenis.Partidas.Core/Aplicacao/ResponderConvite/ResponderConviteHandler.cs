using AgendaTenis.Partidas.Core.Dominio;
using AgendaTenis.Partidas.Core.Enums;
using AgendaTenis.Partidas.Core.Repositorios;

namespace AgendaTenis.Partidas.Core.Aplicacao.ResponderConvite;

public class ResponderConviteHandler
{
    private readonly IPartidasRepositorio _partidaRepositorio;

    public ResponderConviteHandler(IPartidasRepositorio partidaRepositorio)
    {
        _partidaRepositorio = partidaRepositorio;
    }

    public async Task<ResponderConviteResponse> Handle(ResponderConviteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var partida = await _partidaRepositorio.ObterPorIdAsync(request.Id.ToString());

            var notificacoes = ExecutarValidacoes(partida);
            if (notificacoes.Any(c => c.Tipo == Notificacoes.Enums.TipoNotificacaoEnum.Erro || c.Tipo == Notificacoes.Enums.TipoNotificacaoEnum.Aviso))
            {
                return new ResponderConviteResponse()
                {
                    Sucesso = false,
                    Notificacoes = notificacoes
                };
            }

            StatusConviteEnum statusConvite;
            if (request.Aceitar)
            {
                statusConvite = StatusConviteEnum.Aceito;
            }
            else
            {
                statusConvite = StatusConviteEnum.Recusado;
            }

            partida.ResponderConvite(statusConvite);

            var atualizou = await _partidaRepositorio.Update(partida);

            if (atualizou)
            {
                return new ResponderConviteResponse()
                {
                    Sucesso = true
                };
            }
            else
            {
                return new ResponderConviteResponse()
                {
                    Sucesso = false,
                    Notificacoes = new List<Notificacoes.Notificacao>()
                    {
                        new Notificacoes.Notificacao()
                        {
                            Mensagem = "Partida não foi atualizada",
                            Tipo = Notificacoes.Enums.TipoNotificacaoEnum.Aviso
                        }
                    }
                };
            }
        }
        catch (Exception)
        {
            return new ResponderConviteResponse()
            {
                Sucesso = false,
                Notificacoes = new List<Notificacoes.Notificacao>()
                    {
                        new Notificacoes.Notificacao()
                        {
                            Mensagem = "Erro ao atualizar partida",
                            Tipo = Notificacoes.Enums.TipoNotificacaoEnum.Erro
                        }
                    }
            };
        }

    }

    private List<Notificacoes.Notificacao> ExecutarValidacoes(Partida partida)
    {
        var notificacoes = new List<Notificacoes.Notificacao>();

        if (partida is null)
        {
            notificacoes.Add(new Notificacoes.Notificacao()
            {
                Mensagem = "Partida não encontrada",
                Tipo = Notificacoes.Enums.TipoNotificacaoEnum.Aviso
            });

            return notificacoes;
        }

        return notificacoes;
    }
}
