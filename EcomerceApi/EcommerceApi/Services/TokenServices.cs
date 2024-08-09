using System.Security.Cryptography;
using EcommerceApi.Data;
using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

        public void Delete(string token)
        {
            var delToken =  _dbContext.Tokens.FirstOrDefault(t => t.token == token );

            if(delToken != null)
            {
            _dbContext.Tokens.Remove(delToken);
            _dbContext.SaveChangesAsync();
            }
        
        }

        public async Task<Token> Create()
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

    }
}