using ecommerce.sharedLibrary.Response;
using System.Linq.Expressions; // Ensure this using statement is present

namespace ecommerce.sharedLibrary.Interface
{
    public interface IGenricInterface<T> where T : class
    {
        Task<ecommerce.sharedLibrary.Response.Response> CreateAsync(T entity);

        Task<ecommerce.sharedLibrary.Response.Response> UpdateAsync(T entity);

        Task<ecommerce.sharedLibrary.Response.Response> DeletAsync(T entity);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> FindByIdAsync(int id);

       Task<T> GetByAsync (Expression<Func<T, bool>> predicate);
    }
}
