using AgendaTenis.Partidas.Core.Enums;
using AgendaTenis.Partidas.Core.Repositorios;
using Microsoft.Extensions.Caching.Distributed;
using AgendaTenis.Infra.Cache;

namespace AgendaTenis.Partidas.Core.Aplicacao.ConvitesPendentes;

public class ObterConvitesPendentesHandler
{
    private readonly IPartidasRepositorio _partidaRepositorio;
    private readonly IDistributedCache _cache;

    public ObterConvitesPendentesHandler(IPartidasRepositorio partidaRepositorio, IDistributedCache cache)
    {
        _partidaRepositorio = partidaRepositorio;
        _cache = cache;
    }

    public async Task<ObterConvitesPendentesResponse> Handle(ObterConvitesPendentesCommand request, CancellationToken cancellationToken)
    {
        string recordId = $"convitespendentes:{request.UsuarioId}";
        var convites = await _cache.GetRecordAsync<ObterConvitesPendentesResponse>(recordId);

        if (convites is null)
        {
            var partidas = await _partidaRepositorio.ObterPartidasPendentes(request.UsuarioId, StatusConviteEnum.Pendente);

            convites = new ObterConvitesPendentesResponse()
            {
                Partidas = partidas.Select(p => new ObterConvitesPendentesResponse.Partida()
                {
                    Id = p.Id,
                    DesafianteId = p.DesafianteId,
                    AdversarioId = p.AdversarioId,
                    DataDaPartida = p.DataDaPartida,
                    IdCidade = p.Cidade.Id,
                    NomeCidade = p.Cidade.Nome,
                    ModeloDaPartida = p.ModeloDaPartida
                }).ToList()
            };

            await _cache.SetRecordAsync(recordId, convites, TimeSpan.FromMinutes(2));
        }

        return convites;
    }
}
