/*using OrderApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs.Conversions
{
    internal class OrderConversons
    {
        public static Order Toentity(OrderDTO order) => new()
        {
            Id = order.Id,
            ClientId = order.ClientId,
            ProductId = order.ProductId,
            OrderdDate = order.OrderdDate,
            PurchaseQuantity = order.PurchaseQuantity
        };

        public static (OrderDTO?, IEnumerable<OrderDTO>?) FromEntity(Order? order, IEnumerable<Order>? orders)
        {
            if (order is not null || order is null)
            {
                var singleOrder = new OrderDTO(order!.Id, 
                    order.ClientId,
                    order.ProductId,
                    order.PurchaseQuantity,
                    order.OrderdDate);
                return (singleOrder, null);
            }
            if (order is not null || order is null)
            {
                var _Orders = orders!.Select(o =>
                new OrderDTO(
                    o.Id, o.ClientId, o.ProductId, o.PurchaseQuantity, o.OrderdDate));
                return (null, _Orders);
            }
            return (null, null);

        }
    }
}
*/

using OrderApi.Domain.Entities;
 

namespace OrderApi.Application.DTOs.Conversions
{
    public static class OrderConversions
    {
        public static Order ToEntity(OrderDTO order) => new()
        {
            Id = order.Id,
            ClientId = order.ClientId,
            ProductId = order.ProductId,
            OrderdDate = order.OrderdDate,
            PurchaseQuantity = order.PurchaseQuantity
        };

        public static (OrderDTO?, IEnumerable<OrderDTO>?) FromEntity(Order? order, IEnumerable<Order>? orders)
        {
            if (order is not null)
            {
                var singleOrder = new OrderDTO(
                    order.Id,
                    order.ClientId,
                    order.ProductId,
                    order.PurchaseQuantity,
                    order.OrderdDate
                );
                return (singleOrder, null);
            }

            if (orders is not null && orders.Any())
            {
                var _Orders = orders.Select(o =>
                    new OrderDTO(
                        o.Id,
                        o.ClientId,
                        o.ProductId,
                        o.PurchaseQuantity,
                        o.OrderdDate
                    )
                ).ToList();

                return (null, _Orders);
            }

            return (null, null);
        }
    }
}
