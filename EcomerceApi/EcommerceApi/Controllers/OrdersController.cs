using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersControllers : ControllerBase
    {
        private readonly OrdersRepository _ordersRepository;

        public OrdersControllers(OrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public IEnumerable<object> Sql { get; private set; }

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetAll()
        {
            var orders = _ordersRepository.GetAll();
            return Ok(orders);
        }

        [HttpGet("list_orders_all_id")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllItens()
        {   
            var orders = await _ordersRepository.GetAllItens();

            if (orders == null) return NotFound("Not found all Orders");
          
            return Ok(orders);
        }

        [HttpPost("list_orders_id")]
        [Consumes("application/json")]
        public async Task<ActionResult<Order>> List_orders_id([FromBody] ListOrdersIdDto listId)
        {   
            var orders = await _ordersRepository.List_orders_id( listId );

            if(orders == null)
            {
                return NotFound($"id: {listId.id} Not Found!!!");
            }

             return Ok(orders);
        }

        [HttpPost]
        [Consumes("application/json")]
        public ActionResult<Order> Create([FromBody] CreateOrderDto order_to_create)
        {

            var order =  _ordersRepository.Create(order_to_create);

            return Ok(order);
        }
        
    }
}

