using System.ComponentModel.DataAnnotations;

namespace NLayer.Core.DTOs.UserDtos
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public String? SurName { get; set; }
        public string? UserName { get; set; }
        [Required(ErrorMessage = "E-posta adresi alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }


    }
}
