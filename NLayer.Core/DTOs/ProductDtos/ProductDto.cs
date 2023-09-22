using NLayer.Core.Abstract;
using NLayer.Core.DTOs.AnimalDtos;

namespace NLayer.Core.DTOs.ProductDtos
{
    public class ProductDto : BaseDto
    {
  
        public string? SerialNumber { get; set; }
        public string? Code { get; set; }
        public bool Condition { get; set; }
        public string ImageUrl { get; set; }
        public int? AnimalId { get; set; }
        public AnimalDto Animal { get; set; }

    }
}
