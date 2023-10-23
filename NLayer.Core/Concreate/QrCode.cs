using NLayer.Core.Abstract;

namespace NLayer.Core.Concreate
{
    public class QrCode : BaseEntity
    {
        public string? ImageUrl { get; set; }
        public string? SerialNumber { get; set; }
        public string? Code { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }


    }
}
