using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregrate;

namespace Talabat.Core.Specification.order_spec
{
    public class OrderWithPaymentIntentIdSpec : BaseSpecification<Order>
    {
        public OrderWithPaymentIntentIdSpec(string paymentIntnetId) : base(o => o.PaymentIntentId == paymentIntnetId)
        {   
            
            
        }
    }
}
