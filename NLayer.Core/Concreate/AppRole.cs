using Microsoft.AspNetCore.Identity;

namespace NLayer.Core.Concreate
{
    public class AppRole : IdentityRole<int>
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        public Boolean Condition { get; set; }

        public List<UserRole> Users { get; set; }

    }
}
