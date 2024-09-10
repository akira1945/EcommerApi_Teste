using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Repositories
{
    public class SellersRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly TokenServices _tokenServices;

        public SellersRepository(AppDbContext dbContext, TokenServices tokenServices)
        {
            _dbContext = dbContext;
            _tokenServices = tokenServices;
        }

        public async Task<IEnumerable<Seller>> GetAll()
        {
            return await _dbContext.Sellers.ToListAsync();
        }

        public async Task<Seller?> GetResearch(researchSellersDto research)
        {
            return await _dbContext.Sellers.FirstOrDefaultAsync(s => s.id == research.id && s.reference == research.reference);
        }

        public async Task<Seller?> Create(createSellersDto create)
        {
            var existReference = await _dbContext.Sellers.FirstOrDefaultAsync(c => c.reference == create.reference);

            if (existReference != null)
            {
                return null;
            }
            else
            {
                var seller = new Seller
                {
                    name = create.name,
                    reference = create.reference,
                    phone = create.phone,
                    email = create.email,
                    password = create.password
                };

                _dbContext.Add(seller);
                await _dbContext.SaveChangesAsync();

                return seller;

            }


        }

        public async Task<dynamic?> update(updateSellersDto updateSellers)
        {

            var seller = await _dbContext.Sellers.FirstOrDefaultAsync(s => s.id == updateSellers.id);
            if (seller == null)
            {
                return null;
            }

            if (updateSellers.name != null) { seller.name = updateSellers.name; };
            if (updateSellers.phone != null) { seller.phone = updateSellers.phone; };
            if (updateSellers.reference != null) { seller.reference = updateSellers.reference; };
            if (updateSellers.email != null) { seller.email = updateSellers.email; };
            if (updateSellers.password != null) { seller.password = updateSellers.password; };

            await _dbContext.SaveChangesAsync();
            return seller;


        }


        public async Task<Seller?> Delete(deleteSellersDto delete)
        {
            var seller = await _dbContext.Sellers.FirstOrDefaultAsync(s => s.id == delete.id && s.reference == delete.reference);
            if (seller == null)
            {
                return null;
            }
            _dbContext.Sellers.Remove(seller);
            await _dbContext.SaveChangesAsync();
            return seller;

        }

        public async Task<Seller?> DeleteId(int id)
        {
            var seller = await _dbContext.Sellers.FirstOrDefaultAsync(s => s.id == id);
            if (seller == null)
            {
                return null;
            }

            _dbContext.Sellers.Remove(seller);
            await _dbContext.SaveChangesAsync();

            return seller;
        }

        public bool ValidReference(string reference) // Modificar o nome do metodo.
        {

            var valid_reference = _dbContext.Sellers.FirstOrDefault(s => s.reference == reference);

            if (valid_reference != null) { return false; }

            return true;
        }

        public async Task <dynamic?> LoginSeller( LoginSellerDTO login )
        {
            var parameters = new[]
            {
                new Npgsql.NpgsqlParameter("email", login.email),
                new Npgsql.NpgsqlParameter("password", login.password)
            };

            dynamic? user;

             user = await _dbContext.Set<UserDTO>().FromSqlRaw("SELECT * FROM sellers WHERE email = @email AND password = @password LIMIT 1;  ", parameters).FirstOrDefaultAsync();

            if(user == null) return null;

            return user;
        
        }






    }
}