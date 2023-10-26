using System.ComponentModel.DataAnnotations;

namespace NLayer.Core.DTOs.AccountDtos
{
    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }


    }
}
