using API_01.Dtos;
using API_01.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Entities.Order_Aggregrate;
using Talabat.Core.Services;

namespace API_01.Controllers
{

    [Authorize]
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService ,IMapper mapper) 
        {
            _orderService = orderService;
            _mapper = mapper;
        }


        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]

        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail=User.FindFirstValue(ClaimTypes.Email);

            var shippingAdress = _mapper.Map<AddressDto, Address>(orderDto.shipToAddress);

            var order= await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId , orderDto.DeliveryMethodId , shippingAdress);
            if (order == null)
            {
                return BadRequest(new ApiResponse(400));
            }
            var mappedOrder = _mapper.Map<Order, OrderToReturnDto>(order);

            return Ok(mappedOrder);   
        }
                     
        
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrderForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrderForUserAsync(buyerEmail);
            var mappedOrder = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders);
            return Ok(mappedOrder);  
        }


        [HttpGet("{id}")]

        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var order=await _orderService.GetOrderByIdForUserAsync(email,id);
           
            if (order is null) return NotFound(new ApiResponse(400));

            var mappedOrder = _mapper.Map<Order, OrderToReturnDto>(order);


            return Ok(mappedOrder);


        }

        [HttpGet("deliveryMethod")]

        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        {
            var deliveryMehod=await _orderService.GetDeliveryMethodsForUsersAsync();

            return Ok(deliveryMehod);
        }





    }
}
