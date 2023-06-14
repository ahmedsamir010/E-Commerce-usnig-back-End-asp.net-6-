using System.ComponentModel.DataAnnotations;

namespace API_01.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress(ErrorMessage ="Email Is Invalid")]
        public string Email { get; set; }
    }
}
