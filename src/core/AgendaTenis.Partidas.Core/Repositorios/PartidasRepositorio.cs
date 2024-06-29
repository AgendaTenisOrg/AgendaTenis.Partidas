using AgendaTenis.Partidas.Core.Dominio;
using AgendaTenis.Partidas.Core.Enums;
using MongoDB.Driver;

namespace AgendaTenis.Partidas.Core.Repositorios;

public class PartidasRepositorio : IPartidasRepositorio
{
    private readonly IMongoCollection<Partida> _partidasCollection;

    public PartidasRepositorio(IMongoClient client)
    {
        string databaseName = "PartidasDb";
        string collectionName = "PartidasCollection";

        var db = client.GetDatabase(databaseName);
        _partidasCollection = db.GetCollection<Partida>(collectionName);
    }

    public async Task<Partida> ObterPorIdAsync(string id)
    {
        var filter = Builders<Partida>.Filter.Eq(c => c.Id, id);

        var partida = await _partidasCollection.Find(filter).FirstOrDefaultAsync();

        return partida;
    }

    public async Task<List<Partida>> ObterPartidasPendentes(int usuarioId, StatusConviteEnum? statusConvite = null)
    {
        var filter = Builders<Partida>.Filter.And(
                Builders<Partida>.Filter.Or(
                    Builders<Partida>.Filter.Eq(c => c.DesafianteId, usuarioId),
                    Builders<Partida>.Filter.Eq(c => c.AdversarioId, usuarioId)
                 ),
                Builders<Partida>.Filter.Eq(c => c.StatusConvite, statusConvite)
            );

        var partidas = await _partidasCollection.Find(filter).ToListAsync();

        return partidas;
    }

    public async Task<List<Partida>> ObterConfirmacoesDePlacarPendentes(int usuarioId)
    {
        var filter = Builders<Partida>.Filter.And(
                Builders<Partida>.Filter.Eq(c => c.AdversarioId, usuarioId),
                Builders<Partida>.Filter.Eq(c => c.StatusPlacar, StatusPlacarEnum.AguardandoConfirmacao)
            );

        var partidas = await _partidasCollection.Find(filter).ToListAsync();

        return partidas;
    }

    public async Task InsertAsync(Partida partida)
    {
        if (partida is null)
            throw new ArgumentNullException(nameof(partida));

        await _partidasCollection.InsertOneAsync(partida);
    }

    public async Task<bool> Update(Partida partida)
    {
        var filter = Builders<Partida>.Filter.Eq(c => c.Id, partida.Id);
        var result = await _partidasCollection.ReplaceOneAsync(filter, partida);

        return result.ModifiedCount > 0;
    }

    public async Task<List<Partida>> ObterPartidasPaginado(int usuarioId, int pagina, int itensPorPagina)
    {
        var filter = Builders<Partida>.Filter.Or(
                   Builders<Partida>.Filter.Eq(c => c.DesafianteId, usuarioId),
                   Builders<Partida>.Filter.Eq(c => c.AdversarioId, usuarioId)
                );

        int skip = (pagina - 1) * itensPorPagina;
        var partidas = _partidasCollection.Find(filter).Skip(skip).Limit(itensPorPagina).ToList();

        return partidas;
    }
}
