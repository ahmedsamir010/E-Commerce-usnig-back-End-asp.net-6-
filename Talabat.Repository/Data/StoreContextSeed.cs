using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregrate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.ProductBrands.Any())
            {

                var brandData = File.ReadAllText(@"..\Talabat.Repository\Data\DataSeed\brands.json");

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                if (brands != null && brands.Count > 0)
                {

                    foreach (var brand in brands)
                    {
                        await context.Set<ProductBrand>().AddAsync(brand);


                        await context.SaveChangesAsync();

                    }

                }
            }

            if (!context.ProductTypes.Any())
            {

                var TypesData = File.ReadAllText(@"..\Talabat.Repository\Data\DataSeed\types.json");

                var types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);

                if (types != null && types.Count > 0)
                {

                    foreach (var type in types)
                    {
                        await context.Set<ProductType>().AddAsync(type);
                        await context.SaveChangesAsync();
                    }

                }
            }

            if (!context.Products.Any())
            {

                var ProducstData = File.ReadAllText(@"..\Talabat.Repository\Data\DataSeed\products.json");

                var Products = JsonSerializer.Deserialize<List<Product>>(ProducstData);

                if (Products != null && Products.Count > 0)
                {

                    foreach (var Product in Products)
                    {
                        await context.Set<Product>().AddAsync(Product);
                        await context.SaveChangesAsync();
                    }

                }
            }
            if (!context.DeliveryMethods.Any())
            {

                var DeliveryMethodsData = File.ReadAllText(@"..\Talabat.Repository\Data\DataSeed\delivery.json");

                var deliveryMethod = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);

                if (deliveryMethod != null && deliveryMethod.Count > 0)
                {

                    foreach (var delivery in deliveryMethod)
                    {
                        await context.Set<DeliveryMethod>().AddAsync(delivery);
                        await context.SaveChangesAsync();
                    }

                }
            }


        }
    }
}

