using AgendaTenis.Partidas.Core.Aplicacao.ConfirmacoesDePlacarPendentes;
using AgendaTenis.Partidas.Core.Aplicacao.ConvidarParaPartida;
using AgendaTenis.Partidas.Core.Aplicacao.ConvitesPendentes;
using AgendaTenis.Partidas.Core.Aplicacao.HistoricoDePartidas;
using AgendaTenis.Partidas.Core.Aplicacao.RegistrarPlacar;
using AgendaTenis.Partidas.Core.Aplicacao.ResponderConvite;
using AgendaTenis.Partidas.Core.Aplicacao.ResponderPlacar;
using AgendaTenis.Partidas.WebApi.Polices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaTenis.Partidas.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PartidasController : ControllerBase
{
    [HttpGet("Historico")]
    [Authorize]
    public async Task<IActionResult> HistoricoDePartidas([FromServices] ObterHistoricoDePartidasHandler handler, int pagina, int itensPorPagina)
    {
        var errosRequest = new List<object>();

        if (pagina <= 0)
            errosRequest.Add(new { pagina = "pagina deve ser um número inteiro maior do que zero" });

        if (itensPorPagina <= 0)
            errosRequest.Add(new { itensPorPagina = "itensPorPagina deve ser um número inteiro maior do que zero" });

        if (errosRequest.Any())
            return BadRequest(errosRequest);

        var command = new ObterHistoricoDePartidasCommand() { UsuarioId = int.Parse(User.Identity.Name), Pagina = pagina, ItensPorPagina = itensPorPagina };

        var response = await handler.Handle(command, new CancellationToken());
        return Ok(response);
    }

    [HttpPost("Convites/Convidar")]
    [Authorize]
    public async Task<IActionResult> ConvidarParaPartida([FromServices] ConvidarParaPartidaHandler handler, [FromBody] ConvidarParaPartidaCommand command)
    {
        command.DesafianteId = int.Parse(User.Identity.Name);
        var response = await handler.Handle(command, new CancellationToken());
        return Ok(response);
    }

    [HttpGet("Convites/Pendentes")]
    [Authorize]
    public async Task<IActionResult> ConvitesPendentes([FromServices] ObterConvitesPendentesHandler handler)
    {
        var response = await handler.Handle(new ObterConvitesPendentesCommand() { UsuarioId = int.Parse(User.Identity.Name) }, new CancellationToken());
        return Ok(response);
    }

    [HttpPut("Convites/Responder")]
    [Authorize]
    public async Task<IActionResult> ResponderConvite([FromServices] AdversarioDaPartidaPoliceHandler policeHandler, [FromServices] ResponderConviteHandler handler, [FromBody] ResponderConviteCommand command)
    {
        var usuarioEhAdversarioDaPartida = await policeHandler.Validar(command.Id);
        if (!usuarioEhAdversarioDaPartida)
            return Forbid();

        var response = await handler.Handle(command, new CancellationToken());
        return Ok(response);
    }

    [HttpGet("Placar/Pendentes")]
    [Authorize]
    public async Task<IActionResult> ObterConfirmacaoDePlacarPendentes([FromServices] ObterConfirmacoesDePlacarPendentesHandler handler)
    {
        var response = await handler.Handle(new ObterConfirmacoesDePlacarPendentesCommand() { UsuarioId = int.Parse(User.Identity.Name) }, new CancellationToken());
        return Ok(response);
    }

    [HttpPut("Placar/Registrar")]
    [Authorize]
    public async Task<IActionResult> RegistrarPlacar(
        [FromServices] DesafianteDaPartidaPoliceHandler policeHandler,
        [FromServices] RegistrarPlacarHandler handler,
        [FromBody] RegistrarPlacarCommand command)
    {
        var usuarioEhDesafianteDaPartida = await policeHandler.Validar(command.Id);
        if (!usuarioEhDesafianteDaPartida)
            return Forbid();

        var response = await handler.Handle(command, new CancellationToken());
        return Ok(response);
    }

    [HttpPut("Placar/Responder")]
    [Authorize]
    public async Task<IActionResult> ResponderPlacar(
        [FromServices] AdversarioDaPartidaPoliceHandler policeHandler,
        [FromServices] ResponderPlacarHandler handler,
        [FromBody] ResponderPlacarCommand command)
    {
        var usuarioEhAdversarioDaPartida = await policeHandler.Validar(command.Id);
        if (!usuarioEhAdversarioDaPartida)
            return Forbid();

        var response = await handler.Handle(command, new CancellationToken());
        return Ok(response);
    }
}
