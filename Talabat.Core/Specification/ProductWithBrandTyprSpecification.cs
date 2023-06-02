using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class ProductWithBrandTyprSpecification : BaseSpecification<Product>
    {
        // Get All Product
        public ProductWithBrandTyprSpecification(ProductsSpecParams productsSpecParams)
            :base(P => 
                     (string.IsNullOrEmpty(productsSpecParams.Search) || P.Name.ToLower().Contains(productsSpecParams.Search)) && 
                     (!productsSpecParams.BrandId.HasValue || P.ProductBrandId == productsSpecParams.BrandId.Value) &&
                     (!productsSpecParams.BrandId.HasValue || P.ProductTypeId == productsSpecParams.BrandId.Value))

        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
            ApplyPagination(productsSpecParams.PageSize *(productsSpecParams.PageIndex - 1) , productsSpecParams.PageSize);

            if(!string.IsNullOrEmpty(productsSpecParams.Sort))
            {
                switch (productsSpecParams.Sort) 
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);  // Chck This End Point 
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }

            // total size =100
            //   page size =20
            //   page index=3



        }
        public ProductWithBrandTyprSpecification(int id):base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }

    }
}
