using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name  { get; set; }
        public string Description  { get; set; }
        public string PictureUrl  { get; set; }
        public decimal Price  { get; set; }

        [ForeignKey("ProductBrand")]
        public int ProductBrandId { get; set; } // Foriegn Key : Not Allow Null
        
        public ProductBrand ProductBrand { get; set; }  // Navigational Property ==> [ ONE ]
       
        
        [ForeignKey("productType")]
        public int ProductTypeId { get; set; } // Foriegn Key : Not Allow Null

        public List<ProductRating> productRatings { get; set; }

        public ProductType ProductType { get; set; }  // Navigational Property ==> [ ONE ]


    }
}
