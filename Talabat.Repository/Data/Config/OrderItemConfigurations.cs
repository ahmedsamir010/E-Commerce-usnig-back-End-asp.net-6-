using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregrate;

namespace Talabat.Repository.Data.Config
{
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItems>
    {
        public void Configure(EntityTypeBuilder<OrderItems> builder)
        {
            builder.OwnsOne(orderItem => orderItem.Product, Product => Product.WithOwner());

            builder.Property(orderItem => orderItem.Price)
                   .HasPrecision(18, 2);

        }
    }
}
