using NLayer.Core.DTOs.FeatureWithUserDtos.PersonProductFeatureDtos;
using NLayer.Core.DTOs.ProductDtos;

namespace NLayer.Core.DTOs.FeatureDtos.PersonProductFeatureDtos
{
    public class GetByIdWithPersonProductDto : PersonProductFeatureDto
    {
        public ProductDto product { get; set; }
    }
}
