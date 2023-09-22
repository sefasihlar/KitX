using NLayer.Core.DTOs.AnimalDtos;

namespace NLayer.Core.DTOs.ProductDtos
{
    public class GetAnimalWithProductId : ProductDto
    {
        public AnimalDto Animal { get; set; }
    }
}
