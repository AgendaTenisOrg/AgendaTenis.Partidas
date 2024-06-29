using AgendaTenis.Partidas.Core.Repositorios;

namespace AgendaTenis.Partidas.WebApi.Polices;

public class AdversarioDaPartidaPoliceHandler
{
    private readonly IPartidasRepositorio _partidaRepositorio;
    private readonly HttpContext _httpContext;

    public AdversarioDaPartidaPoliceHandler(IPartidasRepositorio partidaRepositorio, IHttpContextAccessor httpContextAccessor)
    {
        _partidaRepositorio = partidaRepositorio;
        _httpContext = httpContextAccessor.HttpContext;
    }

    public async Task<bool> Validar(string partidaId)
    {
        var partida = await _partidaRepositorio.ObterPorIdAsync(partidaId);
        if (partida.AdversarioId == int.Parse(_httpContext.User.Identity.Name))
        {
            return true;
        }

        return false;
    }
}
