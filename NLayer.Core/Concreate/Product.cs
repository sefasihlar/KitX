using NLayer.Core.Abstract;

namespace NLayer.Core.Concreate
{
    public class Product : BaseEntity
    {
        public string? ImageUrl { get; set; }
        public string? SerialNumber { get; set; }
        public string? Code { get; set; }
        public int? AnimalId { get; set; }
        public Animal? Animal { get; set; }

        public List<UserProduct> UserProducts { get; set; }


    }
}
