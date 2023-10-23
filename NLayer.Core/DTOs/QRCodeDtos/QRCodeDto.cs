using NLayer.Core.Abstract;
using NLayer.Core.DTOs.ProductDtos;

namespace NLayer.Core.DTOs.QRCodeDtos
{
    public class QRCodeDto : BaseDto
    {
        public string? ImageUrl { get; set; }
        public string? SerialNumber { get; set; }
        public string? Code { get; set; }
        public int? ProductId { get; set; }
        public ProductDto Product { get; set; }

    }
}
