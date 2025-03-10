using ecommerce.sharedLibrary.Interface;
using OrderApi.Domain.Entities;

using System.Linq.Expressions;


namespace OrderApi.Application.Interfaces
{
    public interface IOrder : IGenricInterface<Order>
    {
        Task<IEnumerable<Order>> GetOrderAsync(Expression<Func<Order, bool>> predicate);
    }
}
