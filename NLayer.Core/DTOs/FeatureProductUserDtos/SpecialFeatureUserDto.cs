using NLayer.Core.DTOs.FeatureWithUserDtos.SpecialProductFeatureDtos;
using NLayer.Core.DTOs.ProductDtos;
using NLayer.Core.DTOs.UserDtos;

namespace NLayer.Core.DTOs.FeatureProductUserDtos
{
    public class SpecialFeatureUserDto : SpecialProductFeatureDto
    {
        public ProductDto Product { get; set; }
        public AppUserDto User { get; set; }
    }
}
