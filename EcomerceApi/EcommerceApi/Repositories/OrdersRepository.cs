using EcommerceApi.Data;

namespace EcommerceApi.Repositories;

public class OrdersRepository
{
    private readonly AppDbContext _dbContext;

    public OrdersRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


}
