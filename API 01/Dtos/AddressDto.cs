using System.ComponentModel.DataAnnotations;

namespace API_01.Dtos
{
    public class AddressDto
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }  
    }
}
