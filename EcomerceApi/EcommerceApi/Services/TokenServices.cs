using System.Security.Cryptography;
using EcommerceApi.Data;
using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql;

namespace  EcommerceApi.Services
{
    public class TokenServices
    {
        private readonly AppDbContext _dbContext;

        public TokenServices(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GenerateToken()
        {
            var randBytes = new byte[4];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(randBytes);
            }; 

            byte[] hash;
            hash = SHA512.HashData(randBytes);

            string encodeHash = Convert.ToBase64String(hash);

            return encodeHash;
        }

        public async Task<Token> GenerateAndSaveToken()
        {
            string generatedToken = this.GenerateToken();

            Console.WriteLine(generatedToken);

            var tokenToCreate = new Token
            {
                token = generatedToken
            };

            _dbContext.Tokens.Add(tokenToCreate);
            await _dbContext.SaveChangesAsync();

            return tokenToCreate;
        }
    
        public void Delete(string token)
        {
            var delToken =  _dbContext.Tokens.FirstOrDefault(t => t.token == token);

            if(delToken != null)
            {
            _dbContext.Tokens.Remove(delToken);
            _dbContext.SaveChangesAsync();
            }
       
        
        }

    }
}

// 1.As rotas de login (clients e sellers) elas ficam dentro do controller de clients e sellers (e os respectivos repositórios); 
// 2.O método de geração de um novo token é INDEPENDENTE de qualquer controller, serviço ou repositório.
// 3.Quem gera o token é o módulo de tokens (serviço). 
// 4.O token deve ser gerado no login (e em todas as outras rotas) como sendo o ÚLTIMO PASSO do controller
// 5.O módulo de tokens NÃO TEM CONTROLLER