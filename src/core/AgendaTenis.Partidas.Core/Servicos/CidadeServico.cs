using AgendaTenis.Partidas.Core.Dominio;
using AgendaTenis.Partidas.Core.DTOs;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace AgendaTenis.Partidas.Core.Servicos;

public class CidadeServico : ICidadeServico, IDisposable
{
    private readonly HttpClient _httpClient;

    public CidadeServico(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CidadeDto> ObterPorId(int id)
    {
        return await _httpClient.GetFromJsonAsync<CidadeDto>($"cidades/obter/{id}", new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }

    public void Dispose() => _httpClient?.Dispose();
}
