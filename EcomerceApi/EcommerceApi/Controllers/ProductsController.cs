using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections;
using System.Reflection.Metadata;
using EcommerceApi.Repositories;


namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepository _productRepository;

        public ProductsController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var product = await _productRepository.GetAll();
            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetId(int id)
        {
            var product = await _productRepository.GetId(id);
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
            var product = await _productRepository.ListPorductSql(listProductSql);

            if (product == null)
            {
                return NotFound("Product Not Found");
            }

            return Ok(product);
        }

        [HttpPost("create_product")]
        [Consumes("application/json")]
        public ActionResult<Product> CreateProduct([FromBody] CreateProductDto createProduct)
        {
            var product = _productRepository.CreateProduct(createProduct);

            return Ok(product);
        }

        [HttpPost("created_product_sql")]
        [Consumes("application/json")]
        public ActionResult<Product> CreateProductSql([FromBody] CreateProductDto createProduct)
        {
            var product = _productRepository.CreateProductSql(createProduct);

            return Ok(product);
        }

        [HttpPost("list_prod_stock")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> ListProduct([FromBody] ListProductStockDto productStock)
        {
            var product = await _productRepository.ListProduct(productStock);
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
            var product = await _productRepository.UpdatePorductString(id, updateProduct);
            if (product == null)
            {
                return NotFound("Product Not Found");
            }
            return Ok(product);
        }

        [HttpPut("alt_prod_price_camp/{id}")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> UpdatePorductPrice(int id, [FromBody] UpdatePriceProductDto updateProduct)
        {
            var product = await _productRepository.UpdatePorductPrice(id, updateProduct);
            if (product == null)
            {
                return NotFound("Product Not Found");
            }
            else if (updateProduct.price < 0)
            {
                return BadRequest($"Product {id} Price {updateProduct.price} Negative");
            }

            return Ok(product);
        }

        [HttpPut("alt_prod_stock_camp/{id}")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> UpdatePorductStock(int id, [FromBody] UpdateStockProductDto updateProduct)
        {
            var product = await _productRepository.UpdatePorductStock(id, updateProduct);
            if (product == null)
            {
                return NotFound("Product Not Found");
            }
            if (updateProduct.stock <= 0)
            {
                return BadRequest("Product Stock Negative or Null");
            }

            return Ok(product);
        }

        [HttpPut("update_prod_sql/{id}")]
        [Consumes("application/json")]
        public ActionResult<Product> UpdateProductSql(int id, [FromBody] UpdateProductSqlDto updateSql)
        {
            var product_update = _productRepository.UpdateProductSql(id, updateSql);

            if (product_update == null)
            {
                return NotFound("Product NOT FOUND");
            }


            return Ok(product_update);
        }

        [HttpDelete("delete_product/{id}")]
        public async Task<ActionResult> DeleteId(int id)
        {
            var product = await _productRepository.DeleteId(id);
            if (product == null) { return NotFound("Product Not Found"); }
            return Ok("Product Deleted Sucesse");
        }

        [HttpPost("delete_sql")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> DeleteSql([FromBody] DeleteSqlDto deleteSql)
        {
            var product_delete = await _productRepository.DeleteSql(deleteSql);
            return Ok(product_delete);
        }


    }
}


