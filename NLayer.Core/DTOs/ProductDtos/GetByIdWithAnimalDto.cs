using NLayer.Core.DTOs.AnimalDtos;

namespace NLayer.Core.DTOs.ProductDtos
{
    public class GetByIdWithAnimalDto : ProductDto
    {
        public AnimalDto? Animal { get; set; }
    }
}
