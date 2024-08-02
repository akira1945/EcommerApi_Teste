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

        // [HttpGet]
        // public ActionResult<IEnumerable<Order>> GetAll()
        // {
        //     var orders = _dbContext.Orders
        //                                 .FromSqlRaw(@"SELECT o.id, o.client_id, o.seller_id, o.delivery_type,
        //                                                     o.order_status, o.total_price, c.name AS client_name,
        //                                                     s.name AS seller_name
        //                                                 FROM orders AS o
        //                                                INNER JOIN clients AS c ON (o.client_id = c.id)
        //                                                INNER JOIN sellers AS s ON (o.seller_id = s.id);");
        //     return Ok(orders);
        // }

        // [HttpGet("list_orders")]
        // public async Task<ActionResult<IEnumerable<Order>>> GetId()
        // {   
        //     var orders = await _dbContext.Orders.FromSqlRaw(@"SELECT o.id, o.client_id, o.seller_id, o.delivery_type,
        //                                                              o.order_status, o.total_price, c.name AS client_name,
        //                                                              s.name AS seller_name
        //                                                        FROM orders AS o
        //                                                       INNER JOIN clients AS c ON (o.client_id = c.id)
        //                                                       INNER JOIN sellers AS s ON (o.seller_id = s.id);
        //                 ")
        //                 .ToListAsync(); 
    
        //    foreach ( var order in orders )
        //    {
        //     var parameters = new []
        //     {
        //         new Npgsql.NpgsqlParameter("order_id", order.id)
        //     };

        //     var order_items = _dbContext.Order_Items
        //                                             .FromSqlRaw(@"SELECT it.id, it.order_id, it.product_id, it.quantity,
        //                                                                  p.price AS product_price, p.title AS product_title
        //                                                             FROM order_items AS it
        //                                                            INNER JOIN products AS p ON (it.product_id = p.id)
        //                                                            WHERE it.order_id = @order_id", parameters);
        //     order.order_Items = order_items;

        //    }
          
        //     return Ok(orders);
        // }

        // [HttpPost("list_orders_id")]
        // [Consumes("application/json")]
        // public async Task<ActionResult<Order>> List_orders_id([FromBody] ListOrdersIdDto listId)
        // {   
        //     var vSql =  $@"SELECT o.id, o.client_id, o.seller_id, o.delivery_type,
        //                           o.order_status, o.total_price, c.name AS client_name,
        //                           s.name AS seller_name
        //                      FROM orders AS o
        //                INNER JOIN clients AS c ON (o.client_id = c.id)
        //                INNER JOIN sellers AS s ON (o.seller_id = s.id)
        //                     WHERE o.id = @id;";

        //     var parameters = new[]
        //     {
        //         new Npgsql.NpgsqlParameter("id",listId.id)
        //     };

        //     var orders = _dbContext.Orders.FromSqlRaw(vSql,parameters).ToList();


        //     foreach ( var order in orders )
        //    {
        //     var parameters_items = new []
        //     {
        //         new Npgsql.NpgsqlParameter("order_id", order.id)
        //     };

        //     var order_items = _dbContext.Order_Items
        //                                             .FromSqlRaw(@"SELECT it.id, it.order_id, it.product_id, it.quantity,
        //                                                                  p.price AS product_price, p.title AS product_title
        //                                                             FROM order_items AS it
        //                                                            INNER JOIN products AS p ON (it.product_id = p.id)
        //                                                            WHERE it.order_id = @order_id", parameters_items);
        //     order.order_Items = order_items;
        //    }

        //     if(orders.Count == 0)
        //     {
        //         return NotFound($"id: {listId.id} Not Found!!!");
        //     }

        //      return Ok(orders);
        // }

        // [HttpPost]
        // [Consumes("application/json")]
        // public async Task<ActionResult<Order>> Create([FromBody] CreateOrderDto order_to_create)
        // {

        //     foreach (var product in order_to_create.products)
        //     {//validar estoque
        //         int stock = ProductRepository.VerifyStock(product.id,product.quantity,  _dbContext);
        //         if(stock <= 0) return BadRequest($"The produc with id {product.id} has no stock");
        //     }

        //     var client = _dbContext.Clients.FromSqlInterpolated($"SELECT * FROM clients WHERE id = {order_to_create.client_id} LIMIT 1").AsEnumerable().FirstOrDefault();

        //     if(client == null) return BadRequest("Client Not Found");

        //     var seller = _dbContext.Sellers.FromSqlInterpolated($"SELECT * FROM sellers WHERE id = {order_to_create.seller_id} LIMIT 1").ToList();

        //     if(seller.Count <= 0) return BadRequest("Seller not found");

        //     Double num = 0.00;

        //     var parameters = new []
        //     {
        //         new Npgsql.NpgsqlParameter("client_id", order_to_create.client_id),
        //         new Npgsql.NpgsqlParameter("seller_id", order_to_create.seller_id),
        //         new Npgsql.NpgsqlParameter("delivery_type", order_to_create.delivery_type),
        //         new Npgsql.NpgsqlParameter("order_status", "pending_confirmation"),
        //         new Npgsql.NpgsqlParameter("total_price", num)
        //     };

        //     var order = _dbContext.Orders.FromSqlRaw(@"
        //         INSERT INTO orders (client_id, seller_id, delivery_type, order_status, total_price)
        //         VALUES (@client_id, @seller_id, @delivery_type, @order_status, @total_price)
        //         RETURNING * 
        //         ", parameters).AsEnumerable().FirstOrDefault();
        //         //fazendo a atribuição manual dos campos que não existe no Model.
        //         order.client_name = client.name;
        //         order.seller_name = seller[0].name;

        // return Ok(order);
        // }
        
    }
}

//cria uma rota responsavel por criar um novo pedido
// - Receber todos os itens desse pedido, valor e quantidade
// - receber o cliente do pedido
// - receber o vendedor desse pedido

// - verifica se o produto tem estoque
// - Calcular o preço do peiddo [{price: 10, quantity: 2 total_price: 20} , {price: 5, quantity: 10 total_price: 500}]
// - Inserir na tabela de order_items cada produto do pedido.