using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class CustomerBasket
    {
        public string Id { get; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }

        public decimal ShippingCost { get; set; }
        public CustomerBasket(string id)
        {
            Id = id;
        }
    }
}