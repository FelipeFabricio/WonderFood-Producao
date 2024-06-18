using MassTransit;
using WonderFood.Application.Common;
using WonderFood.Models.Events;

namespace WonderFood.Worker.Consumers;

public class IniciarProducaoConsumer(IPedidoService pedidoService) : IConsumer<IniciarProducaoCommand>
{
    public async Task Consume(ConsumeContext<IniciarProducaoCommand> context)
    {
        await pedidoService.ReceberPedidosParaPreparo(context.Message);
    }
}