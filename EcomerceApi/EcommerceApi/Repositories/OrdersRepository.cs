using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Repositories;

public class OrdersRepository
{
    private readonly AppDbContext _dbContext;

    public OrdersRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Order> GetAll()
        {
            var orders = _dbContext.Orders
                                        .FromSqlRaw(@"SELECT o.id, o.client_id, o.seller_id, o.delivery_type,
                                                            o.order_status, o.total_price, c.name AS client_name,
                                                            s.name AS seller_name
                                                        FROM orders AS o
                                                       INNER JOIN clients AS c ON (o.client_id = c.id)
                                                       INNER JOIN sellers AS s ON (o.seller_id = s.id);");
            return orders;
        }

    public async Task<IEnumerable<Order>> GetAllItens()
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
            order.order_items = (ICollection<Order_item>)order_items;

           }
           
           if(orders == null){ return null; }
          
            return orders;
        }



    public async Task<List<Order?>?> List_orders_id(ListOrdersIdDto listId)
        {
            var vSql = $@"SELECT o.id, o.client_id, o.seller_id, o.delivery_type,
                                    o.order_status, o.total_price, c.name AS client_name,
                                    s.name AS seller_name
                                FROM orders AS o
                        INNER JOIN clients AS c ON (o.client_id = c.id)
                        INNER JOIN sellers AS s ON (o.seller_id = s.id)
                                WHERE o.id = @id;";

            var parameters = new[]
            {
                    new Npgsql.NpgsqlParameter("id",listId.id)
                };

            var orders = await _dbContext.Orders.FromSqlRaw(vSql, parameters).ToListAsync();

            foreach (var order in orders)
            {
                var parameters_items = new[]
                {
                    new Npgsql.NpgsqlParameter("order_id", order.id)
                };

                var order_items =await _dbContext.Order_Items
                                                            .FromSqlRaw(@"SELECT it.id, it.order_id, it.product_id, it.quantity,
                                                                                p.price AS product_price, p.title AS product_title
                                                                            FROM order_items AS it
                                                                        INNER JOIN products AS p ON (it.product_id = p.id)
                                                                        WHERE it.order_id = @order_id", parameters_items).ToListAsync();
                order.order_items = order_items;
            }

            if ( orders.Any() )
            {
                return null;
            }

            return orders;
        }
    
    public Order? Create( CreateOrderDto order_to_create )
        {

            foreach (var product in order_to_create.products)
            {//validar estoque
                int stock = ProductRepository.VerifyStock(product.id,product.quantity,  _dbContext);
                if(stock < 0 ) return null;
            }

            var client = _dbContext.Clients.FromSqlInterpolated($"SELECT * FROM clients WHERE id = {order_to_create.client_id} LIMIT 1").AsEnumerable().FirstOrDefault();

            if(client == null) return null;

            var seller = _dbContext.Sellers.FromSqlInterpolated($"SELECT * FROM sellers WHERE id = {order_to_create.seller_id} LIMIT 1").ToList();

            if(seller.Count <= 0) return null;

            Double num = 0.00;

            var parameters = new []
            {
                new Npgsql.NpgsqlParameter("client_id", order_to_create.client_id),
                new Npgsql.NpgsqlParameter("seller_id", order_to_create.seller_id),
                new Npgsql.NpgsqlParameter("delivery_type", order_to_create.delivery_type),
                new Npgsql.NpgsqlParameter("order_status", "pending_confirmation"),
                new Npgsql.NpgsqlParameter("total_price", num)
            };

            var order = _dbContext.Orders.FromSqlRaw(@"
                INSERT INTO orders (client_id, seller_id, delivery_type, order_status, total_price)
                VALUES (@client_id, @seller_id, @delivery_type, @order_status, @total_price)
                RETURNING * 
                ", parameters).AsEnumerable().FirstOrDefault();
                //fazendo a atribuição manual dos campos que não existe no Model.
                order.client_name = client.name;
                order.seller_name = seller[0].name;

        return order;
        }

    



}


//cria uma rota responsavel por criar um novo pedido
// - Receber todos os itens desse pedido, valor e quantidade
// - receber o cliente do pedido
// - receber o vendedor desse pedido

// - verifica se o produto tem estoque
// - Calcular o preço do peiddo [{price: 10, quantity: 2 total_price: 20} , {price: 5, quantity: 10 total_price: 500}]
// - Inserir na tabela de order_items cada produto do pedido.