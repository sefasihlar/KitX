using System.ComponentModel.DataAnnotations;

namespace NLayer.Core.DTOs.AccountDtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Kullanıcı adı boş geçilemez.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre boş geçilemez.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
