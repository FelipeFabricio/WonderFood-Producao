using Microsoft.AspNetCore.Mvc;
using WonderFood.Application.Common;
using WonderFood.Domain.Entities.Enums;

namespace WonderFood.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController(IPedidoService pedidoService) : ControllerBase
{
    /// <summary>
    /// Endpoint que altera o status do Pedido
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    [HttpPost("altera-status-pedido")]
    public async Task<IActionResult> AlterarStatusPedido(int numeroPedido, StatusPedido status, string? motivoCancelamento = null)
    {
        try
        {
            if (status != StatusPedido.Cancelado) 
                motivoCancelamento = null;
            
            await pedidoService.AlterarStatusPedido(numeroPedido, status, motivoCancelamento);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { e.Message });
        }
    }
}