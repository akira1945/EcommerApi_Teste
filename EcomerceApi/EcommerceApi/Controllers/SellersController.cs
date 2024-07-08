using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Data;
using EcommerceApi.SellersDTOs;
using EcommerceApi.Models;
using EcommerceApi.SellersServices;
using Microsoft.EntityFrameworkCore;

namespace SellersController
{
    [ApiController]
    [Route("sellers")]

    public class SellersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;        
        public SellersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seller>>> GetAll()
        {
            var sellers = await _dbContext.Sellers.ToListAsync();
            return Ok(sellers);
        }

        [HttpGet]
        [Consumes("application/json")]
        public async Task<ActionResult<Seller>> GetResearch([FromBody] researchSellersDto researchSellers)
        {
            var sellers = await _dbContext.Sellers.FirstOrDefaultAsync( s => s.id == researchSellers.id && s.reference == researchSellers.reference);
            if(sellers == null)
            {
                return NotFound( $"Vendedor(a) não encontrado. Id: {researchSellers.id} e a Referencia: {researchSellers.reference} invalidados!");
            }
            return Ok(sellers);
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<Seller>> Create([FromBody] createSellersDto createSellers)
        {    var existReference = await _dbContext.Sellers.AnyAsync(s => s.reference == createSellers.reference);
             if(existReference)
             {         
                return BadRequest($@"Referencia: {createSellers.reference}, não pode ser utilizada, já existe um cadastro para sequencia! Ultima sequencial utilizado : {EcommerceApi.SellersServices.SellersServices.Empty}");
             };

            var seller = new Seller
            {
                name = createSellers.name, 
                reference = createSellers.reference, 
                phone = createSellers.phone, 
                email = createSellers.email, 
                password = createSellers.password
            };           
            
            _dbContext.Sellers.Add(seller);
            await _dbContext.SaveChangesAsync();
            return Ok(seller);
        }

        [HttpPut]
        [Consumes("application/json")]
        public async Task<ActionResult<Seller>> updateSellers([FromBody] updateSellersDto updateSellers)
        {
            var seller = await _dbContext.Sellers.FirstOrDefaultAsync( s => s.id == updateSellers.id);
            if(seller == null)
            {
                return NotFound($"Cadastro de vendedor não encontrado, por favor, valide o Id: {updateSellers.id} !!!");
            }
            else
            {
                if(updateSellers.name != null){seller.name = updateSellers.name;};
                if(updateSellers.reference != null){seller.reference = updateSellers.reference;};
                if(updateSellers.phone != null){seller.phone = updateSellers.phone;};
                if(updateSellers.email != null){seller.email = updateSellers.email;};
                if(updateSellers.password != null){seller.password = updateSellers.password;};
            }
            await _dbContext.SaveChangesAsync();
            return Ok(seller);
        }
        [HttpDelete]
        [Consumes("application/json")]
        public async Task<ActionResult<Seller>> Delete([FromBody] deleteSellersDto deleteSellers)
        {
            var seller = await _dbContext.Sellers.FirstOrDefaultAsync( s => s.id == deleteSellers.id && s.reference == deleteSellers.reference );
            if(seller == null)
            {
                return NotFound($"Cadastro não encontrado, valide se o id: {deleteSellers.id} e a Referencia: {deleteSellers.reference}, estão corretas!");
            }
            _dbContext.Sellers.Remove(seller);
            await _dbContext.SaveChangesAsync();
            return Ok($"Cadastro excluido com sucesso!!! Referencia {deleteSellers.reference}, pode ser utilizada novamente.");
        }
       
    }
}