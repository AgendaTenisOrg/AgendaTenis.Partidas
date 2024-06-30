using AgendaTenis.Partidas.Core.Enums;
using System.Text.Json.Serialization;

namespace AgendaTenis.Partidas.Core.Aplicacao.ConvidarParaPartida;

public class ConvidarParaPartidaCommand
{
    [JsonIgnore]
    public int DesafianteId { get; set; }
    public int AdversarioId { get; set; }
    public DateTime DataDaPartida { get; set; }
    public int IdCidade { get; set; }
    public string NomeCidade { get; set; }
    public ModeloPartidaEnum ModeloDaPartida { get; set; }
}
