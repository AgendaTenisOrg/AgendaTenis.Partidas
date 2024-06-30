﻿using AgendaTenis.Infra.Eventos.Mensagens;
using AgendaTenis.Partidas.Core.Dominio;
using AgendaTenis.Partidas.Core.Enums;
using AgendaTenis.Partidas.Core.Eventos.Publishers;
using AgendaTenis.Partidas.Core.Repositorios;

namespace AgendaTenis.Partidas.Core.Aplicacao.ResponderPlacar;

public class ResponderPlacarHandler
{
    private readonly IPartidasRepositorio _partidaRepositorio;
    private readonly PlacarConfirmadoPublisher _publisher;

    public ResponderPlacarHandler(
        IPartidasRepositorio partidaRepositorio,
        PlacarConfirmadoPublisher publisher)
    {
        _partidaRepositorio = partidaRepositorio;
        _publisher = publisher;
    }

    public async Task<ResponderPlacarResponse> Handle(ResponderPlacarCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var partida = await _partidaRepositorio.ObterPorIdAsync(request.Id);

            var notificacoes = ExecutarValidacoes(partida);
            if (notificacoes.Any(c => c.Tipo == Notificacoes.Enums.TipoNotificacaoEnum.Erro || c.Tipo == Notificacoes.Enums.TipoNotificacaoEnum.Aviso))
            {
                return new ResponderPlacarResponse()
                {
                    Sucesso = false,
                    Notificacoes = notificacoes
                };
            }

            // Só vamos publicar o evento de Placar confirmado se o status for aceito.
            // Se um dos jogadores contestar o placar,
            // Então não iremos considerar a partida como válida e o evento não será publicado
            if (request.ConfirmarPlacar)
            {
                partida.ResponderPlacar(StatusPlacarEnum.Aceito);

                var mensagem = BuildMensagem(partida);
                _publisher.Publish(mensagem);
            }
            else
            {
                partida.ResponderPlacar(StatusPlacarEnum.Aceito);
            }

            var atualizou = await _partidaRepositorio.Update(partida);

            if (atualizou)
            {
                return new ResponderPlacarResponse()
                {
                    Sucesso = true
                };
            }
            else
            {
                return new ResponderPlacarResponse()
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
            return new ResponderPlacarResponse()
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

        if (partida.StatusPlacar != StatusPlacarEnum.AguardandoConfirmacao)
            notificacoes.Add(new Notificacoes.Notificacao()
            {
                Mensagem = "Não é possível atualizar o placar, pois o status não está Aguardando Confirmação.",
                Tipo = Notificacoes.Enums.TipoNotificacaoEnum.Aviso
            });

        return notificacoes;
    }

    private PlacarConfirmadoMensagem BuildMensagem(Partida partida)
    {
        return new PlacarConfirmadoMensagem()
        {
            Id = partida.Id,
            DesafianteId = partida.DesafianteId,
            AdversarioId = partida.AdversarioId,
            DataDaPartida = partida.DataDaPartida,
            ModeloDaPartida = (int)partida.ModeloDaPartida,
            VencedorId = partida.VencedorId
        };
    }
}