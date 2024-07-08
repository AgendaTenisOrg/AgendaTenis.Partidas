using AgendaTenis.Partidas.Core.DTOs;

namespace AgendaTenis.Partidas.Core.Servicos;

public interface ICidadeServico
{
    Task<CidadeDto> ObterPorId(int id);
}
