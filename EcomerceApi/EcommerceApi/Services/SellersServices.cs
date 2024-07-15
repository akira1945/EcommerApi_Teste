using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Data;
using EcommerceApi.DTOs;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.Models;
using System.Linq.Expressions;
using EcommerceApi.Controller;

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
    
public async Task<ActionResult<string>> GetNextReferenceAsync()
{
    var parameters = new []
    {
        new Npgsql.NpgsqlParameter("reference", NpgsqlTypes.NpgsqlDbType.Text) { Value = "some_reference_value" } // Aqui você deve definir o valor correto para a referência
    };

    var vSql = @"SELECT s.reference FROM sellers AS s WHERE s.reference = @reference;";

    var existRef = await _dbContext.Sellers.FromSqlRaw(vSql, parameters).ToListAsync();

    if (existRef.Any())
    {
        var vSql_newReference = @"SELECT MAX(reference) FROM sellers;";
        var newReference = await _dbContext.Sellers.FromSqlRaw(vSql_newReference).Select(s => s.reference).FirstOrDefaultAsync();

        return Ok(newReference);
    }

    return NotFound("Reference not found");
}

    }
}

        // Busca todas as referências no banco de dados e extrai os números
        //  var maxNumber = await _dbContext.Sellers
        //                                     .Where(s => s.reference.StartsWith("REF"))
        //                                     .Select(s => int.Parse(s.reference.Substring(3))) 
        //                                     .DefaultIfEmpty(0) 
        //                                     .MaxAsync();       
        
        //  int nextNumber = maxNumber + 1;