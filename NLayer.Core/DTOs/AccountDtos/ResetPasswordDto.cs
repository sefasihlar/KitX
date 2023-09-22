using System.ComponentModel.DataAnnotations;

namespace NLayer.Core.DTOs.AccountDtos
{
    public class ResetPasswordDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string RePassword { get; set; }

    }
}
