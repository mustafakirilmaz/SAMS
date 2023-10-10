using System.ComponentModel.DataAnnotations;
using Xunit.Abstractions;

namespace SAMS.Data.Dtos.User
{
    public class ForgetPasswordDto
    {

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string Email { get; set; }
    }
}
