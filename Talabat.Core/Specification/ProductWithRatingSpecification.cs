using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class ProductWithRatingSpecification : BaseSpecification<ProductRating>
    {
        public ProductWithRatingSpecification(int productId ,string userEmail) : base(p => p.ProductId == productId && p.Email == userEmail)
        {
                
        }
    }
}
