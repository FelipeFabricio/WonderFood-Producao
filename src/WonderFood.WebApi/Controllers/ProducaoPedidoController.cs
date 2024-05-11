using Microsoft.AspNetCore.Mvc;
using WonderFood.Application.Common;
using WonderFood.Domain.Entities.Enums;

namespace WonderFood.WebApi.Controllers;

[ApiController]
[Route("[controller]/api")]
public class ProducaoPedidoController(IPedidoService pedidoService) : ControllerBase
{
    /// <summary>
    /// Endpoint que altera o status do Pedido
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    [HttpPost("altera-status-pedido")]
    public async Task<IActionResult> AlterarStatusPedido(int numeroPedido, StatusPedido status)
    {
        try
        {
            await pedidoService.AlterarStatusPedido(numeroPedido, status);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(400, new { e.Message});
        }
    }
}