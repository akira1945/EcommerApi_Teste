using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Services;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.ProductsDTOs;

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
            if(product == null)
            {
                return NotFound("Não temos produtos cadastrados!!!");
            }
            return Ok(product);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetId(int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == id);
            if(product == null)
            {
                return NotFound("Produto não cadastrado!!!");
            }           
            return Ok(product);
        }
       
        [HttpPost]
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
      
        [HttpPost("list_prod_stock")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> ListProduct([FromBody] ListProductStockDto productStock)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync( p => p.id == productStock.id || p.title == productStock.title );
            if(product == null)
            {
                return NotFound($"Produto pesquisado não encontrado!!! Valide se o ID: {productStock.id} ou o titulo {productStock.title} é valido!");
            }
            else if(product.stock <= 0)
            {
                return Ok($"Produto: {product.id}\nDescrição: {product.title}\nEstoque: {product.stock} - Estoque negativo ou zerado.");
            }
             return Ok($"Produto: {product.id}\nDescrição: {product.title}\nEstoque: {product.stock}");
        }
        
        [HttpPut("alt_prod_string_camp/{id}")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> UpdatePorductString(int id,[FromBody] UpdateProductDto updateProduct)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == id);
            if(product == null)
            {
                return NotFound($"Produto pesquisado não encontrado!!! Valide se o ID: {id}");
            }
            else 
            {
                if(updateProduct.title != null && updateProduct.title != product.title) {product.title = updateProduct.title;};
                if(updateProduct.description != null && updateProduct.description != product.description ) {product.description = updateProduct.description;};
                if(updateProduct.image_url != null && updateProduct.image_url != product.image_url ) {product.image_url = updateProduct.image_url;};
            }
            await _dbContext.SaveChangesAsync();
            return Ok(product);
        }
       
        [HttpPut("alt_prod_price_camp/{id}")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> UpdatePorductPrice(int id,[FromBody] UpdatePriceProductDto updateProduct)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == id);
            if(product == null)
            {
                return NotFound($"Produto pesquisado não encontrado!!! Valide se o ID: {id}");
            }
            else if(product.price < 0 )
            {
                return BadRequest($"Produto não pode ter preço negativo !!! Valide se o Estoque: {product.price}");
            }
            else 
            {
                if(updateProduct.price != null && updateProduct.price != product.price) {product.price = (float)updateProduct.price; };
            }
            await _dbContext.SaveChangesAsync();
            return Ok(product);
        }
        
        [HttpPut("alt_prod_stock_camp/{id}")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> UpdatePorductStock(int id,[FromBody] UpdateStockProductDto updateProduct)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == id);
            if(product == null)
            {
                return NotFound($"Produto pesquisado não encontrado!!! Valide se o ID: {id}");
            }
            else if(product.stock < 0 )
            {
                return BadRequest($"Produto não pode ter estoque meno que 0 !!! Valide se o Estoque: {product.stock}");
            }
            else 
            {
                if(updateProduct.stock != null && updateProduct.stock != product.stock) {product.stock = (int)updateProduct.stock; };
            }
            await _dbContext.SaveChangesAsync();
            return Ok(product);
        }

        [HttpDelete("delete_product/{id}")]
        public async Task<ActionResult> DeleteId(int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(s => s.id == id);
            if(product ==  null)
            {
                return NotFound("Exclusão não executada, valide o ID");
            }
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return Ok("Cadstro do produto deletado!!!");
        }



    }
}


