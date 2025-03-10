
using OrderApi.Application.DTOs;
namespace OrderApi.Application.Services
{
    public interface IOrderServices
    {
       Task<IEnumerable<OrderDTO>> GetOrderByClientId(int clientId);
        Task<OrderDetailsDTO> GetOrderDetails(int orderId);
    }
}
