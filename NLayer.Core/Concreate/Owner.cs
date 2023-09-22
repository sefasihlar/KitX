using NLayer.Core.Abstract;

namespace NLayer.Core.Concreate
{
    public class Owner : BaseEntity
    {
        public string? NameSurname { get; set; }
        public string? Email { get; set; }
        public string? Adress { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

    }
}
