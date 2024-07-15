using EcommerceApi.Repositories;
using EcommerceApi.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async  Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
    }

}