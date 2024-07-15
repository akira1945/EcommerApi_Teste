using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections;
using System.Reflection.Metadata;


namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ProductsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var product = await _dbContext.Products.ToListAsync();
            if (product == null)
            {
                return NotFound("Product Not Found");
            }
            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetId(int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == id);
            if (product == null)
            {
                return NotFound("Product Not Found");
            }
            return Ok(product);
        }

        [HttpPost("list_product_sql")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<Product>>> ListPorductSql([FromBody] ListProductSqlDto listProductSql)
        {
            var vSql = $@"SELECT * FROM products WHERE id = @id ;";

            var parameters_get = new[]
            {
                new Npgsql.NpgsqlParameter("id", listProductSql.id)
            };

            var product_sql = await _dbContext.Products.FromSqlRaw(vSql, parameters_get).ToListAsync();

            if (product_sql == null || product_sql.Count == 0)
            {
                return NotFound("Product Not Found");
            }

            return Ok(product_sql);
        }

        [HttpPost("create_product")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDto createProduct)
        {
            var product = new Product
            {
                title = createProduct.title,
                description = createProduct.description,
                image_url = createProduct.image_url,
                price = createProduct.price,
                stock = createProduct.stock,
            };
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPost("created_product_sql")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> CreateProductSql([FromBody] CreateProductDto createProduct)
        {

            var sql_minus_url = $@"
                INSERT INTO products (title, description, price, stock)
                VALUES (@title, @description, @price, @stock)
                RETURNING *;";

            if (createProduct.image_url == null)
            {
                var parameters_incomplete = new[]
                {
                new Npgsql.NpgsqlParameter("title", createProduct.title),
                new Npgsql.NpgsqlParameter("description", createProduct.description),
                new Npgsql.NpgsqlParameter("price", createProduct.price),
                new Npgsql.NpgsqlParameter("stock", createProduct.stock)
            };

                var product_incomplete = _dbContext.Products.FromSqlRaw(sql_minus_url, parameters_incomplete).AsEnumerable();

                return Ok(product_incomplete);
            }

            var sql_url = $@"
                INSERT INTO products (title, description, price, image_url, stock)
                VALUES (@title, @description, @price, @image_url, @stock)
                RETURNING *;";

            var parameters = new[]
            {
                new Npgsql.NpgsqlParameter("title", createProduct.title),
                new Npgsql.NpgsqlParameter("description", createProduct.description),
                new Npgsql.NpgsqlParameter("price", createProduct.price),
                new Npgsql.NpgsqlParameter("image_url", createProduct.image_url),
                new Npgsql.NpgsqlParameter("stock", createProduct.stock)
            };

            var product = _dbContext.Products.FromSqlRaw(sql_url, parameters).AsEnumerable();

            return Ok(product);

            ;
        }

        [HttpPost("list_prod_stock")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> ListProduct([FromBody] ListProductStockDto productStock)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == productStock.id || p.title == productStock.title);
            if (product == null)
            {
                return NotFound("Product Not Found");
            }
            else if (product.stock <= 0)
            {
                return Ok("Product stock Negative or Null");
            }
            return Ok(product);
        }

        [HttpPut("alt_prod_string_camp/{id}")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> UpdatePorductString(int id, [FromBody] UpdateProductDto updateProduct)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == id);
            if (product == null)
            {
                return NotFound("Product Not Found");
            }
            else
            {
                if (updateProduct.title != null && updateProduct.title != product.title) { product.title = updateProduct.title; };
                if (updateProduct.description != null && updateProduct.description != product.description) { product.description = updateProduct.description; };
                if (updateProduct.image_url != null && updateProduct.image_url != product.image_url) { product.image_url = updateProduct.image_url; };
            }
            await _dbContext.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPut("alt_prod_price_camp/{id}")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> UpdatePorductPrice(int id, [FromBody] UpdatePriceProductDto updateProduct)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == id);
            if (product == null)
            {
                return NotFound("Product Not Found");
            }
            else if (product.price < 0)
            {
                return BadRequest("Product Price Negative or Null");
            }
            else
            {
                if (updateProduct.price != null && updateProduct.price != product.price) { product.price = (float)updateProduct.price; };
            }
            await _dbContext.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPut("alt_prod_stock_camp/{id}")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> UpdatePorductStock(int id, [FromBody] UpdateStockProductDto updateProduct)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == id);
            if (product == null)
            {
                return NotFound("Product Not Found");
            }
            else if (updateProduct.stock < 0)
            {
                return BadRequest("Product Stock Negative or Null");
            }
            else
            {
                if (updateProduct.stock != null && updateProduct.stock != product.stock) { product.stock = (int)updateProduct.stock; };
            }
            await _dbContext.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPut("update_prod_sql/{id}")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> UpdateProductSql(int id,[FromBody] UpdateProductSqlDto updateSql)
        {
            var updates = new List<String>();
            var parameters = new List<Npgsql.NpgsqlParameter>            
            {
                new Npgsql.NpgsqlParameter("id" , id),               
            };
             
             if(updateSql.title != null)
             {
                updates.Add("title = @title");                
                parameters.Add(new Npgsql.NpgsqlParameter("title" , updateSql.title));
             }
             if(updateSql.description != null)
             {
                updates.Add("description = @description");                
                parameters.Add(new Npgsql.NpgsqlParameter("description" , updateSql.description));
             }
             if(updateSql.image_url != null)
             {
                updates.Add("image_url = @image_url");                
                parameters.Add(new Npgsql.NpgsqlParameter("image_url" , updateSql.image_url));
             }
             if(updateSql.price != null)
             {
                updates.Add("price = @price");                
                parameters.Add(new Npgsql.NpgsqlParameter("price" , updateSql.price));
             }
             if(updateSql.stock != null)
             {
                updates.Add("stock = @stock");                
                parameters.Add(new Npgsql.NpgsqlParameter("stock" , updateSql.stock));
             }

             if(updates.Count == 0)
             {
                return NotFound("Update Invalid!");
             }

             var setUpdate = string.Join(", ", updates);

             var vSql = $@"UPDATE products
                              SET {setUpdate}
                            WHERE id = @id RETURNING *;";
            
             var product_update = _dbContext.Products.FromSqlRaw(vSql, parameters.ToArray()).AsEnumerable().FirstOrDefault();

             if(product_update == null)
             {
                return NotFound("Product NOT FOUND");
             }


            return Ok(product_update);
        }

        [HttpDelete("delete_product/{id}")]
        public async Task<ActionResult> DeleteId(int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(s => s.id == id);
            if (product == null)
            {
                return NotFound("Product Not Found");
            }
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return Ok("Product Deleted Sucesse");
        }

        [HttpPost("delete_sql")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> DeleteSql([FromBody] DeleteSqlDto deleteSql)
        {
            var sql_delete = $@"DELETE FROM products WHERE id = @id RETURNING *;";

            var param_delete = new[] { new Npgsql.NpgsqlParameter("id", deleteSql.id) };

            var product_delete = _dbContext.Products.FromSqlRaw(sql_delete, param_delete).AsEnumerable().FirstOrDefault();

            await _dbContext.SaveChangesAsync();

            return Ok(product_delete);
        }



    }
}


