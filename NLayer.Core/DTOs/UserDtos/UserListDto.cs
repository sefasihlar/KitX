using NLayer.Core.Concreate;

namespace NLayer.Core.DTOs.UserDtos
{
    public class UserListDto : AppUserDto
    {
        public List<AppUser> Users { get; set; }
    }
}
