using System;
using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class ProductWithBrandTyprSpecification : BaseSpecification<Product>
    {
        private readonly Expression<Func<Product, bool>> _criteria;

        public ProductWithBrandTyprSpecification(ProductsSpecParams productsSpecParams)
            : base(p =>
                (string.IsNullOrEmpty(productsSpecParams.Search) ||
                p.Name.ToLower().Contains(productsSpecParams.Search)) &&
                (!productsSpecParams.BrandId.HasValue || p.ProductBrandId == productsSpecParams.BrandId.Value) &&
                (!productsSpecParams.TypeId.HasValue || p.ProductTypeId == productsSpecParams.TypeId.Value))
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
            Includes.Add(R => R.productRatings);
            ApplyPagination(productsSpecParams.PageSize * (productsSpecParams.PageIndex - 1), productsSpecParams.PageSize);

            if (!string.IsNullOrEmpty(productsSpecParams.Sort))
            {
                switch (productsSpecParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }

        }
        public ProductWithBrandTyprSpecification(int id)
            : base(p => p.Id == id)
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
            Includes.Add(R => R.productRatings);
        }



    }
}
