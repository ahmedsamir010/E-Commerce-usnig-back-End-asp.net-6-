using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregrate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specification.order_spec;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork , IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            var orderItems = new List<OrderItems>();
            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                    var prouctItemOrdered = new ProductItemOrdered(item.Id, item.ProductName, item.PictureUrl);

                    var orderItem = new OrderItems(prouctItemOrdered, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            var deliverMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var spec = new OrderWithPaymentIntentIdSpec(basket.PaymentIntentId);

            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
    
            if (existingOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                                                                          // PaymentIntnet  هيتعمل امتي لاول مره ؟
                 await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }
   

            var order = new Order(buyerEmail, shippingAddress, deliverMethod, orderItems, subTotal, basket.PaymentIntentId);

            await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.Complete();

            return order;
        }




        public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
        {
            if (string.IsNullOrEmpty(buyerEmail))
                throw new ArgumentException("Buyer email cannot be null or empty.", nameof(buyerEmail));

            var spec = new Order_Spec(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithAsync(spec);

            return orders;
        }


        public async Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
        {
            if (string.IsNullOrEmpty(buyerEmail))
                throw new ArgumentException("Buyer email cannot be null or empty.", nameof(buyerEmail));

            if (orderId <= 0)
                throw new ArgumentException("Order ID must be greater than zero.", nameof(orderId));

            var spec = new Order_Spec(buyerEmail, orderId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsForUsersAsync()
        {
            var deliverMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

            return deliverMethod;
        }
    }
}
