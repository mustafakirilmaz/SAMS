using System.ComponentModel.DataAnnotations;

namespace SAMS.Data.Dtos
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string NewPasswordConfirm { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string Guid { get; set; }
    }
}
