using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities;

namespace AdminDashboard.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }

        public IFormFile Image { get; set; }

        public string? PictureUrl { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        [Range(1, 100000)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "ProductTypeId is Required")]
        public int ProductTypeId { get; set; }
        public ProductType? ProductType { get; set; }

        [Required(ErrorMessage = "ProductBrandId is Required")]

        public int ProductBrandId { get; set; }
        public ProductBrand? ProductBrand { get; set; }
    }   
}
