using NLayer.Core.Abstract;

namespace NLayer.Core.Concreate
{
    public class Category : BaseEntity
    {
        public string? Name { get; set; }
        public List<Product> Products { get; set; }

    }
}
