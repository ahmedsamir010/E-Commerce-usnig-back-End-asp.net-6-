using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class ProductRating : BaseEntity
    {
        public int ProductId { get; set; }
        public string UserName { get; set; }
        public double RatingValue { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
