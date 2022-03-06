using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using System.Threading.Tasks;

namespace Ordering.Api.MessageBusConsumers
{
    
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediatr;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketCheckoutConsumer> _logger;

        public BasketCheckoutConsumer(IMediator mediatr, IMapper mapper, ILogger<BasketCheckoutConsumer> logger)
        {
            _mediatr = mediatr;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var order = _mapper.Map<CheckoutOrderCommand>(context.Message);
            var result = await _mediatr.Send(order);
            _logger.LogInformation($"BasketCheckoutEvent is consumed successfully , order with Id={result} created");
        }
    }
}
