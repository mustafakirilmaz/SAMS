using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Data.Dtos
{
    public class UserDto
    {
        public long? Id { get; set; }

        public string Password { get; set; }

        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public string Phone { get; set; }

        public string ImageUrl { get; set; }

        public string RowGuid { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public List<long> RoleIds { get; set; }

        public string OldEmail { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public bool? IsActive { get; set; }

    }
}
