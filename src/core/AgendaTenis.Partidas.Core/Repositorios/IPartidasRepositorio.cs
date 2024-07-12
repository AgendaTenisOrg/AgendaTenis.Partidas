using AgendaTenis.Partidas.Core.Dominio;
using AgendaTenis.Partidas.Core.Enums;

namespace AgendaTenis.Partidas.Core.Repositorios;

public interface IPartidasRepositorio
{
    Task<Partida> ObterPorIdAsync(string id);
    Task<List<Partida>> ObterPartidasPendentes(int usuarioId, StatusConviteEnum? statusConvite = null);
    Task<List<Partida>> ObterConfirmacoesDePlacarPendentes(int usuarioId);
    Task<List<Partida>> ObterPartidasPaginado(int usuarioId, int pagina, int itemsPorPagina);
    Task InsertAsync(Partida partida);
    Task<bool> Update(Partida partida);
    Task<int> ObterTotalDePartidas(int usuarioId);
}
