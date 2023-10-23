using NLayer.Core.Abstract;

namespace NLayer.Core.Concreate
{
    public class Location : BaseEntity
    {
        public string? Latitude { get; set; }
        public string? Lonqitude { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
