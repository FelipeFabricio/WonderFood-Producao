using Microsoft.AspNetCore.Mvc;
using WonderFood.Application.Common;
using WonderFood.Models.Events;

namespace WonderFood.Worker.Webhooks;

[Route("/webhook/pedidos")]
public class PedidosWebhook(IPedidoService pedidoService) : ControllerBase
{
    /// <summary>
    /// Webhook que recebe o solicitação do início da produção de um pedido.
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    [HttpPost("iniciar-pedido")]
    public async Task<IActionResult> IniciarProducaoPedido([FromBody] IniciarProducaoCommand payload)
    {
        try
        {
            await pedidoService.ReceberPedidosParaPreparo(payload);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(400, new { e.Message});
        }
    }
}