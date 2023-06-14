using System.ComponentModel.DataAnnotations;

namespace API_01.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required] 
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        
        public string Password { get; set; }

    }
}
