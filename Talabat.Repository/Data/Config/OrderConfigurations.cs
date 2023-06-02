using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregrate;

namespace Talabat.Repository.Data.Config
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());

            builder.Property(o => o.Status)
                .HasConversion(
                    Ostatus => Ostatus.ToString(),
                    Ostatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), Ostatus)
                );

            builder.Property(o => o.Subtotal)
                   .HasColumnType("decimal(18,2)");

            builder.HasMany(o => o.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
