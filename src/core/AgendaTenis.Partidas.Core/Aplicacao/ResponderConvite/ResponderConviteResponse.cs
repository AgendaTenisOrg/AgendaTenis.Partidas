﻿using AgendaTenis.Notificacoes;

namespace AgendaTenis.Partidas.Core.Aplicacao.ResponderConvite;

public class ResponderConviteResponse
{
    public bool Sucesso { get; set; }
    public List<Notificacao> Notificacoes { get; set; }
}