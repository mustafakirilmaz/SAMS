using System.ComponentModel.DataAnnotations;

namespace SAMS.Data.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string Password { get; set; }
    }
}
