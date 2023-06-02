using System.ComponentModel.DataAnnotations;

namespace API_01.Dtos
{
    public class ProductRatingDto
    {
        public int ProductId { get; set; }
        [Range(1, 5)]
        public double RatingValue { get; set; }
        public string? Message { get; set; }

    }
}