using Talabat.Core.Entities.Order_Aggregrate;

namespace API_01.Dtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; } 

        public Address ShippingAddress { get; set; }

        //public DeliveryMethod DeliveryMethod { get; set; }  // Naviational Property [ ONE ]
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }


        public ICollection<OrderItemDto> Items { get; set; }  // Naviational Property [ Many ]
        public decimal Subtotal { get; set; }


        public string PaymentIntentId { get; set; }

        public decimal Total { get; set; }

    }
}
