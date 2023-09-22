using Microsoft.AspNetCore.Identity;

namespace NLayer.Core.Concreate
{
    public class AppUser : IdentityUser<int>
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public bool Condition { get; set; }

        public List<UserProduct> UserProducts { get; set; }

    }
}
