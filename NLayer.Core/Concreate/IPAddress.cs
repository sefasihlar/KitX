using NLayer.Core.Abstract;

namespace NLayer.Core.Concreate
{
    public class IPAddress : BaseEntity
    {
        public string? IPAdress { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
