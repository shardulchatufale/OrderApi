using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using OrderApi.Application.DTOs;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using OrderApi.Application.DTOs.Conversions;
using System.Collections.Generic;
using Azure;
using MySqlX.XDevAPI;


namespace OrderApi.presetation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrder orderInterface, IOrderServices orderServices) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrder()
        {
            var order = await orderInterface.GetAllAsync();
            if (!order.Any()) return NotFound("No order detected");

            var (_, list) = OrderConversions.FromEntity(null, order);
            return !list!.Any() ? NotFound() : Ok(list);

        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await orderInterface.FindByIdAsync(id);

            if (order == null) return NotFound(null);
            var (_order, _) = OrderConversions.FromEntity(order, null);
            return Ok(_order);
        }










        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Incomplete data submitted");
            }
            var getEntity = OrderConversions.ToEntity(orderDTO);
            var response = await orderInterface.CreateAsync(getEntity);
            return response.Flag ? Ok(response) : BadRequest(response);

        }



        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO orderDTO)
        {
            var oreder = OrderConversions.ToEntity(orderDTO);
            var response = await orderInterface.UpdateAsync(oreder);
            return response.Flag ? Ok(response) : BadRequest(response);
        }


        [HttpDelete]
        public async Task<ActionResult<Response>> deleteOrder(OrderDTO orderDTO)
        {
            var order = OrderConversions.ToEntity(orderDTO);
            var response = await orderInterface.DeletAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }





        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<OrderDTO>> GetClientOrder(int clientId)
        {
            if (clientId <= 0) return BadRequest("Invalid Data Provided");

            var orders = await orderServices.GetOrderByClientId(clientId);
            return !orders.Any()? NotFound(null):Ok(orders);
        }



        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDetailsDTO>>GetOrderDetail(int orderId)
        {
            if (orderId <= 0) return BadRequest("Invalid Data Provided");
            var OrderDetail = await orderServices.GetOrderDetails(orderId);
            return OrderDetail.OrderId > 0 ? Ok(OrderDetail) : NotFound("No orderFound");
        }


    }
}
