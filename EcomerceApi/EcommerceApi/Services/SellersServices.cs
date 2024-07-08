using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Controllers;
using EcommerceApi.Data;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.Models;

namespace EcommerceApi.SellersServices
{
    [ApiController]
    [Route("services")]
    public class SellersServices : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public SellersServices(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    
    [HttpGet]
     public virtual async Task<ActionResult<Seller>> GetNextReferenceAsync(string newReference)
        {
        // Busca todas as referências no banco de dados e extrai os números
        var maxNumber = await _dbContext.Sellers
            .Where(s => s.reference.StartsWith("REF"))
            .Select(s => int.Parse(s.reference.Substring(3))) // Extrai o número da referência
            .DefaultIfEmpty(0) // Se não houver referências, usa 0
            .MaxAsync();

        // Incrementa o número máximo encontrado
        int nextNumber = maxNumber + 1;
         
        // Gera a nova referência no formato "REF0001"
         newReference = $"REF{nextNumber:D4}";

            return Ok($"Nova Referencia {newReference}");      
        }
        
        
    }
}