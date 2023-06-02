using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregrate
{
    public class OrderItems :BaseEntity
    {
        public OrderItems()
        {
            
        }
        public OrderItems(ProductItemOrdered product, decimal price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOrdered Product { get; set; }
        public decimal Price { get; set;}
        public int Quantity { get; set;}



    }
}
    