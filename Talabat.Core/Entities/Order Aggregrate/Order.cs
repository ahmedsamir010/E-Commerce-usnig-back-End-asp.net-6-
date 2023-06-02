using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregrate
{
    public class Order :BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItems> items, decimal subtotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Address ShippingAddress { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }  // Naviational Property [ ONE ]

        public ICollection<OrderItems> Items { get; set; } =new HashSet<OrderItems>(); // Naviational Property [ Many ]
        public decimal Subtotal { get; set; }

        //public decimal Total { get; set; }   // Derived Attribute 

        public decimal GetTotal()
            => Subtotal + DeliveryMethod.Cost;

        public string? PaymentIntentId { get; set; } 

    }
}
