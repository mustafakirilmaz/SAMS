using System.ComponentModel.DataAnnotations;

namespace SAMS.Data.Dtos
{
    public class ChangePasswordProfileDto
	{
        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public long? UserId { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string NewPasswordConfirm { get; set; }
    }
}
