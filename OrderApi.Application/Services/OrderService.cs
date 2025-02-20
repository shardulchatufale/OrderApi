using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using Polly;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface, HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipeline) : IOrderServices
    {
        public async Task<ProductDTO> GateProduct(int ProductId)
        {
            var getProduct = await httpClient.GetAsync($"/api/products/{ProductId}");
            if (!getProduct.IsSuccessStatusCode) { return null!; }

            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;

        }

        public async Task<AppUserDTO> GetUser(int userId)
        {
            var getUser = await httpClient.GetAsync($"/api/product/{userId}");
            if (!getUser.IsSuccessStatusCode) { return null!; }

            var product = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return product!;
        }
        public async Task<OrderDetailsDTO> GetOrderDetails(int orderId)
        {
            var order = await orderInterface.FindByIdAsync(orderId);
            if (order is null || order!.Id <= 0) { return null!; }

            var retryPipeLine = resiliencePipeline.GetPipeline("my-retry-pipeline");

            var productDTO = await retryPipeLine.ExecuteAsync(async token => await GateProduct(order.ProductId));

            var appUserDTO = await retryPipeLine.ExecuteAsync(async token => await GetUser(order.ClientId));

            return new OrderDetailsDTO(
                order.Id, productDTO.Id, appUserDTO.Id, appUserDTO.Name, appUserDTO.Email, appUserDTO.Address
               , appUserDTO.TelephoneNumber, productDTO.Name, order.PurchaseQuantity, productDTO.Price,
                productDTO.Quantity * productDTO.Price, order.OrderdDate);
        }


        public async Task<IEnumerable<OrderDTO>> GetOrderByClientId(int clientId)
        {
            var order = await orderInterface.GetOrderAsync(o => o.ClientId == clientId);
            if (order.Any()) { return null; }
           var (_, orders) = OrderConversions.FromEntity(null, order);
            return orders!;
        }

        
    }
}
