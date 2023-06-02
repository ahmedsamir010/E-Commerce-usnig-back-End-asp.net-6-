using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregrate;

namespace Talabat.Core.Specification.order_spec
{
    public class Order_Spec :BaseSpecification<Order>
    {
        public Order_Spec(string email) : base(o => o.BuyerEmail == email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

            AddOrderByDesc(o => o.OrderDate);
        }
        public Order_Spec(string email , int orderId) : base(o => o.BuyerEmail == email && o.Id == orderId)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }

    }
}
