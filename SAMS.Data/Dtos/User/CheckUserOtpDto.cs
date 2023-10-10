using System.ComponentModel.DataAnnotations;
using Xunit.Abstractions;

namespace SAMS.Data.Dtos.User
{
    public class CheckUserOtpDto
    {

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string OtpCode { get; set; }
    }
}
