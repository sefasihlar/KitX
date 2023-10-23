using NLayer.Core.Abstract;

namespace NLayer.Core.Concreate
{
    public class ProductPhoto : BaseEntity
    {
        public string? ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
