using NLayer.Core.Abstract;

namespace NLayer.Core.DTOs.UserDtos
{
    public class AppUserDto : BaseDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Mail { get; set; }
        public string UserName { get; set; }
        public string? Phone { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public bool Condition { get; set; }

    }
}
