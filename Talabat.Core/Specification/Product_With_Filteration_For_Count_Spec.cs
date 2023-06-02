using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class Product_With_Filteration_For_Count_Spec :BaseSpecification<Product>
    {
        public Product_With_Filteration_For_Count_Spec(ProductsSpecParams productsSpecParams)
             : base(P =>
                     (string.IsNullOrEmpty(productsSpecParams.Search) || P.Name.ToLower().Contains(productsSpecParams.Search)) &&
                     (!productsSpecParams.BrandId.HasValue || P.ProductBrandId == productsSpecParams.BrandId.Value) &&
                     (!productsSpecParams.BrandId.HasValue || P.ProductTypeId == productsSpecParams.BrandId.Value))
        {


            
        }


    }
}
