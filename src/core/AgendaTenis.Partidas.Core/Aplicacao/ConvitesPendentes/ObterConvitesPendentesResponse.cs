﻿using AgendaTenis.Partidas.Core.Enums;

namespace AgendaTenis.Partidas.Core.Aplicacao.ConvitesPendentes;

public class ObterConvitesPendentesResponse
{
    public List<Partida> Partidas { get; set; }

    public class Partida
    {
        public string Id { get; set; }
        public int DesafianteId { get; set; }
        public int AdversarioId { get; set; }
        public DateTime DataDaPartida { get; set; }
        public string DescricaoLocal { get; set; }
        public ModeloPartidaEnum ModeloDaPartida { get; set; }
    }
}
