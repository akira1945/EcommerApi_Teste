using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Repositories;

public class ProductRepository
{

    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>?> GetAll()
        {
            var product = await _dbContext.Products.ToListAsync();
            if (product == null)
            {
                return null;
            }
            return product;
        }

     public async Task<Product?> GetId(int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == id);
            if (product == null)
            {
                return null;
            }
            return product;
        }
    
     public async Task<IEnumerable<Product>?> ListPorductSql( ListProductSqlDto listProductSql)
        {
            var vSql = $@"SELECT * FROM products WHERE id = @id ;";

            var parameters_get = new[]
            {
                new Npgsql.NpgsqlParameter("id", listProductSql.id)
            };

            var product_sql = await _dbContext.Products.FromSqlRaw(vSql, parameters_get).ToListAsync();

            if (product_sql == null || product_sql.Count == 0)
            {
                return null;
            }

            return product_sql;
        }
      public Product CreateProduct( CreateProductDto createProduct )
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
            _dbContext.SaveChangesAsync();
            return product;
        }
     public Product CreateProductSql( CreateProductDto createProduct )
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

                var product_incomplete = _dbContext.Products.FromSqlRaw(sql_minus_url, parameters_incomplete).AsEnumerable().FirstOrDefault();

                return product_incomplete;

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

            var product = _dbContext.Products.FromSqlRaw(sql_url, parameters).AsEnumerable().FirstOrDefault();

            return product;

            ;
        }
    
    public async Task<Product?> ListProduct( ListProductStockDto productStock )
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == productStock.id || p.title == productStock.title);
            if (product == null)
            {
                return null;
            }
             return product;
        }

    public async Task<Product?> UpdatePorductString(int id, UpdateProductDto updateProduct)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == id);
            if (product == null)
            {
                return null;
            }
            else
            {
                if (updateProduct.title != null && updateProduct.title != product.title) { product.title = updateProduct.title; };
                if (updateProduct.description != null && updateProduct.description != product.description) { product.description = updateProduct.description; };
                if (updateProduct.image_url != null && updateProduct.image_url != product.image_url) { product.image_url = updateProduct.image_url; };
            }
            await _dbContext.SaveChangesAsync();
            return product;

        } 

    public async Task<Product?> UpdatePorductPrice(int id, UpdatePriceProductDto updateProduct)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == id);
            if (product == null)
            {
                return null;
            }
            
            if( updateProduct.price > 0 ) { product.price = (float)updateProduct.price; };
            
            await _dbContext.SaveChangesAsync();
            return product;
        }   

    public async Task<Product?> UpdatePorductStock(int id, UpdateStockProductDto updateProduct)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == id);
            if (product == null)
            {
                return null;
            }
           
            //condição ? valor_se_verdadeiro : valor_se_falso;
            product.stock = updateProduct.stock > 0 ? (int)updateProduct.stock + product.stock : product.stock;

            await _dbContext.SaveChangesAsync();
            return product;
        }
    public Product? UpdateProductSql(int id,  UpdateProductSqlDto updateSql)
    {
        var updates = new List<String>();
        var parameters = new List<Npgsql.NpgsqlParameter>
        {
            new Npgsql.NpgsqlParameter("id" , id),
        };

        if (updateSql.title != null)
        {
            updates.Add("title = @title");
            parameters.Add(new Npgsql.NpgsqlParameter("title", updateSql.title));
        }
        if (updateSql.description != null)
        {
            updates.Add("description = @description");
            parameters.Add(new Npgsql.NpgsqlParameter("description", updateSql.description));
        }
        if (updateSql.image_url != null)
        {
            updates.Add("image_url = @image_url");
            parameters.Add(new Npgsql.NpgsqlParameter("image_url", updateSql.image_url));
        }
        if (updateSql.price != null && updateSql.price > 0)
        {
            updates.Add("price = @price");
            parameters.Add(new Npgsql.NpgsqlParameter("price", updateSql.price));
        }
        if (updateSql.stock != null && updateSql.stock >0 )
        {
            updates.Add("stock = @stock");
            parameters.Add(new Npgsql.NpgsqlParameter("stock", updateSql.stock));
        }

        if (updates.Count == 0)
        {
            return null;
        }

        var setUpdate = string.Join(", ", updates);

        var vSql = $@"UPDATE products
                            SET {setUpdate}
                        WHERE id = @id RETURNING *;";

        var product_update = _dbContext.Products.FromSqlRaw(vSql, parameters.ToArray()).AsEnumerable().FirstOrDefault();

        if (product_update == null)
        {
            return null;
        }


        return product_update;
    }

    public async Task<Product?> DeleteId(int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(s => s.id == id);
            if (product == null)
            {
                return null ;
            }
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return product ;
        }

    public async Task<Product?> DeleteSql( DeleteSqlDto deleteSql )
        {
            var sql_delete = $@"DELETE FROM products WHERE id = @id RETURNING *;";

            var param_delete = new[] { new Npgsql.NpgsqlParameter("id", deleteSql.id) };

            var product_delete = _dbContext.Products.FromSqlRaw(sql_delete, param_delete).AsEnumerable().FirstOrDefault();

            if ( product_delete == null ) return null;

            await _dbContext.SaveChangesAsync();

            return product_delete ;
        }

     public static int VerifyStock(int id, int quantity, AppDbContext dbContext)
        {
            var product = dbContext.Products.FirstOrDefault(p => p.id == id);

            return product?.stock >= quantity ? product.stock : 0;
        }    


}
