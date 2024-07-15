using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersControllers : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public OrdersControllers(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<object> Sql { get; private set; }

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetAll()
        {
            var orders = _dbContext.Orders
                                        .FromSqlRaw(@"SELECT o.id, o.client_id, o.seller_id, o.delivery_type,
                                                            o.order_status, o.total_price, c.name AS client_name,
                                                            s.name AS seller_name
                                                        FROM orders AS o
                                                       INNER JOIN clients AS c ON (o.client_id = c.id)
                                                       INNER JOIN sellers AS s ON (o.seller_id = s.id);");
            return Ok(orders);
        }

        [HttpGet("list_orders_items")]
        public async Task<ActionResult<IEnumerable<Order>>> GetId()
        {   
            var orders = await _dbContext.Orders.FromSqlRaw(@"SELECT o.id, o.client_id, o.seller_id, o.delivery_type,
                                                                     o.order_status, o.total_price, c.name AS client_name,
                                                                     s.name AS seller_name
                                                               FROM orders AS o
                                                              INNER JOIN clients AS c ON (o.client_id = c.id)
                                                              INNER JOIN sellers AS s ON (o.seller_id = s.id);
                        ")
                        .ToListAsync(); 
    
           foreach ( var order in orders )
           {
            var parameters = new []
            {
                new Npgsql.NpgsqlParameter("order_id", order.id)
            };

            var order_items = _dbContext.Order_Items
                                                    .FromSqlRaw(@"SELECT it.id, it.order_id, it.product_id, it.quantity,
                                                                         p.price AS product_price, p.title AS product_title
                                                                    FROM order_items AS it
                                                                   INNER JOIN products AS p ON (it.product_id = p.id)
                                                                   WHERE it.order_id = @order_id", parameters);
            order.order_Items = order_items;

           }

            return Ok(orders);
        }

        [HttpGet("list_orders_items/id")]
        public async Task<ActionResult<Order>> GetId(int id)
        {   
            var orders =  _dbContext.Orders.FromSqlInterpolated($@"SELECT o.id, o.client_id, o.seller_id, o.delivery_type,
                                                                     o.order_status, o.total_price, c.name AS client_name,
                                                                     s.name AS seller_name
                                                                         FROM orders AS o
                                                                        INNER JOIN clients AS c ON (o.client_id = c.id)
                                                                        INNER JOIN sellers AS s ON (o.seller_id = s.id)
                                                                        WHERE o.id = {id};
                        ").AsEnumerable().ToList();
    
           foreach ( var order in orders )
           {
            var parameters = new []
            {
                new Npgsql.NpgsqlParameter("order_id", order.id)
            };

            var order_items = _dbContext.Order_Items
                                                    .FromSqlRaw(@"SELECT it.id, it.order_id, it.product_id, it.quantity,
                                                                         p.price AS product_price, p.title AS product_title
                                                                    FROM order_items AS it
                                                                   INNER JOIN products AS p ON (it.product_id = p.id)
                                                                   WHERE it.order_id = @order_id", parameters);
            order.order_Items = order_items;

           }

            return Ok(orders);
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<Order>> Create([FromBody] CreateOrderDto order_to_create)
        {

            foreach (var product_id in order_to_create.products)
            {
                
            }

            return Ok("falta");
        }
        
    }
}

//cria uma rota responsavel por criar um novo pedido
// - Receber todos os itens desse pedido, valor e quantidade
// - receber o cliente do pedido
// - receber o vendedor desse pedido

// - verifica se o produto tem estoque
// - Calcular o preço do peiddo [{price: 10, quantity: 2 total_price: 20} , {price: 5, quantity: 10 total_price: 500}]