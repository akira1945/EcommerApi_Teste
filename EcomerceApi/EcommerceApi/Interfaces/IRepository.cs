namespace EcommerceApi.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
    }
}