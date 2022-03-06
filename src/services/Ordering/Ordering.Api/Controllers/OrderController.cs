using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public OrderController(IMediator mediatr)
        {
            _mediatr = mediatr ?? throw new ArgumentNullException(nameof(mediatr));

        }


        [HttpGet("GetOrdersByUserName/{userName}")]
        public async Task<ActionResult<IEnumerable<OrdersVm>>> GetOrdersByUserName(string userName)
        {
            var query=new GetOrdersListQuery(userName);
            var orders = await _mediatr.Send(query);
            return Ok(orders);
        }

        //this will be called by rabbitMq later
        [HttpPost("CheckoutOrder")]
        public async Task<ActionResult<int>> CheckoutOrder(CheckoutOrderCommand order)
        {
            var result=await _mediatr.Send(order);
            return Ok(result);
        }

        [HttpPut("UpdateOrder")]
        public async Task<ActionResult> UpdateOrder(UpdateOrder updateOrderCommand)
        {
            await _mediatr.Send(updateOrderCommand);

            return NoContent();
        }

        [HttpDelete("DeleteOrder/{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrder { Id = id };
            await _mediatr.Send(command);
            return NoContent();

        }
    }
}
