using System.ComponentModel.DataAnnotations;

namespace AdminPanal.Models
{
    public class RoleFormView
    {
        [Required(ErrorMessage ="Name Is Required")]
        [StringLength(256)]
        public string Name { get; set; }
    }
}
