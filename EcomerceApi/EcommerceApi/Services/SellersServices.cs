using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Controllers;
using EcommerceApi.Data;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.Models;

namespace EcommerceApi.Services
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
    
     public async Task<string> GetNextReferenceAsync()
        {
        // Busca todas as referências no banco de dados e extrai os números
         var maxNumber = await _dbContext.Sellers
            .Where(s => s.reference.StartsWith("REF"))
            .Select(s => int.Parse(s.reference.Substring(3))) 
            .DefaultIfEmpty(0) 
            .MaxAsync();       
        
         int nextNumber = maxNumber + 1;
         
        
         string newReference = $"REF{nextNumber:D4}";

            return newReference;      
        }
        
        
    }
}