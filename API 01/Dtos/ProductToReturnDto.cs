using System.ComponentModel.DataAnnotations.Schema;
using Talabat.Core.Entities;

namespace API_01.Dtos
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("ProductBrand")]
        public int ProductBrandId { get; set; } 

        public string ProductBrand { get; set; }  


        [ForeignKey("productType")]
        public int ProductTypeId { get; set; }
        public List<ProductRatingDto> productRatings { get; set; }

        public double AverageRating { get; set; } 
        public string ProductType { get; set; } 
    }
}
