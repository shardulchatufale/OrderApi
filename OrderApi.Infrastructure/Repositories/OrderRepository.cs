using ecommerce.sharedLibrary.Logs;
using ecommerce.sharedLibrary.Response;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entities;
using OrderApi.Infrastructure.Data;

using System.Linq.Expressions;


namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepository(OrderDBContext context) : IOrder
    {
        public async Task<Response> CreateAsync(Order entity)
        {
            try
            {
                var order = context.Orders.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (order.Id > 0)
                {
                    return new Response(true, "Order Placed successfully");
                }
                return new Response(false, "Error occured while placing order");
            }
            catch (Exception ex) 
            { 
                 LogException.LogExceptions(ex);
                return new Response(false, "Error occured while placing order");
            }
        }

        public async  Task<Response> DeletAsync(Order entity)
        {
            try
            {
                var order = await FindByIdAsync(entity.Id);
                if (order is null) { return new Response(false, "ID not found"); }
                context.Orders.Remove(order);
                await context.SaveChangesAsync();
                return new Response(true, "Order is cancelled");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occured while deleting order");
            }
        }

        public async Task<Order> FindByIdAsync(int id)
        {
            try
            {
                var order= await context.Orders!.FindAsync(id);
                return order is not null ? order : null!;
                
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occured while retrieving order");
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var order = await context.Orders.AsNoTracking().ToListAsync();
                return order is not null? order : null!;

            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception( "Error occured while getting order");
            }
        }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order=await context.Orders.Where(predicate).FirstOrDefaultAsync()!;
                return order is not null? order : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
               throw new Exception( "Error occured while retrieving order");
            }
        }

        public  async Task<IEnumerable<Order>> GetOrderAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var orders= await context.Orders.Where(predicate).ToListAsync();
                return orders is not null ? orders : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception( "Error occured while gating order");
            }
        }

        public async Task<Response> UpdateAsync(Order entity)
        {
            try
            {
                var order = await FindByIdAsync(entity.Id);
                if (order is  null)
                {
                    return new Response(false,"Order not found");
                }
                context.Entry(order).State=EntityState.Detached;
                 context.Orders.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, "Order Updated"); 
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception( "Error occured while updating order");
            }
        }
    }
}
