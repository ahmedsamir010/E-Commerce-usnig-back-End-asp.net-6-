using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Config
{
	internal class ProductBrandConfiguration : IEntityTypeConfiguration<ProductBrand>
	{
		public void Configure(EntityTypeBuilder<ProductBrand> builder)
		{
			builder.ToTable("ProductBrands"); // Sets the table name explicitly (optional)

			builder.HasKey(b => b.Id); // Sets the primary key for the entity

			builder.Property(b => b.Name)
				.IsRequired()
				.HasMaxLength(100); // Configures the Name property to be required and have a maximum length of 100 characters

			builder.HasIndex(b => b.Name)
				.IsUnique(); // Creates a unique index on the Name property

			// Additional configuration options can be added as needed
		}
	}
}
