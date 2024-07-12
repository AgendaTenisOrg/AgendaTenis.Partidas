using AgendaTenis.Partidas.Core.Repositorios;
using System.Linq;

namespace AgendaTenis.Partidas.Core.Aplicacao.HistoricoDePartidas;

public class ObterHistoricoDePartidasHandler
{
    private readonly IPartidasRepositorio _partidaRepositorio;

    public ObterHistoricoDePartidasHandler(IPartidasRepositorio partidaRepositorio)
    {
        _partidaRepositorio = partidaRepositorio;
    }

    public async Task<ObterHistoricoDePartidasResponse> Handle(ObterHistoricoDePartidasCommand request, CancellationToken cancellationToken)
    {
        if (request.Pagina <= 0)
        {
            throw new ArgumentOutOfRangeException("Página deve ser um número inteiro maior do que zero");
        }

        if (request.ItensPorPagina <= 0)
        {
            throw new ArgumentOutOfRangeException("Página deve ser um número inteiro maior do que zero");
        }

        var partidas = await _partidaRepositorio.ObterPartidasPaginado(request.UsuarioId, request.Pagina, request.ItensPorPagina);

        var total = await _partidaRepositorio.ObterTotalDePartidas(request.UsuarioId);

        return new ObterHistoricoDePartidasResponse()
        {
            Partidas = partidas.Select(p => new ObterHistoricoDePartidasResponse.Partida()
            {
                Id = p.Id,
                DesafianteId = p.DesafianteId,
                DesafianteNome = p.DesafianteNome,
                AdversarioId = p.AdversarioId,
                AdversarioNome = p.AdversarioNome,
                DataDaPartida = p.DataDaPartida,
                IdCidade = p.Cidade.Id,
                NomeCidade = p.Cidade.Nome,
                ModeloDaPartida = new Enums.ModeloPartidaEnumModel(p.ModeloDaPartida),
                StatusConvite = new Enums.StatusConviteEnumModel(p.StatusConvite),
                StatusPlacar = new Enums.StatusPlacarEnumModel(p.StatusPlacar.GetValueOrDefault()),
                VencedorId = p.VencedorId,
                Sets = p.Sets?.Select(p => new ObterHistoricoDePartidasResponse.Set()
                {
                    NumeroSet = p.NumeroSet,
                    GamesDesafiante = p.GamesDesafiante,
                    GamesAdversario = p.GamesAdversario,
                    TiebreakDesafiante = p.TiebreakDesafiante,
                    TiebreakAdversario = p.TiebreakAdversario
                })
            }),
            TotalDeItens = total
        };
    }
}
