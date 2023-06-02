using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Config
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
          //  builder.Property(P => P.Id).IsRequired();
          builder.Property(N => N.Name).IsRequired().HasMaxLength(100);
            builder.Property(P => P.PictureUrl).IsRequired(false);




            builder.HasOne(P => P.ProductBrand).WithMany()
                   .HasForeignKey(p => p.ProductBrandId);

            builder.HasOne(P => P.ProductType).WithMany()
                   .HasForeignKey(p => p.ProductTypeId);

        }
    }
}
