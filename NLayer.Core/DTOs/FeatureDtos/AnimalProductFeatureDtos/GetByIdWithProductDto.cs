using NLayer.Core.DTOs.FeatureDtos.AnimalProductFeatureDtos;
using NLayer.Core.DTOs.ProductDtos;

namespace NLayer.Core.DTOs.FeatureWithUserDtos.AnimalProductFeatureDtos
{
    public class AnimalFeatureProductUserDto : AnimalProductFeatureDto
    {
        public ProductDto Product { get; set; }

    }
}
