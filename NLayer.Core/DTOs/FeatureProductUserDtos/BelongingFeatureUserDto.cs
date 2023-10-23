using NLayer.Core.DTOs.FeatureWithUserDtos.BelongingProductFeatureDtos;
using NLayer.Core.DTOs.ProductDtos;
using NLayer.Core.DTOs.UserDtos;

namespace NLayer.Core.DTOs.FeatureProductUserDtos
{
    public class BelongingFeatureUserDto : BelongingProductFeatureDto
    {
        public ProductDto Product { get; set; }
        public AppUserDto User { get; set; }
    }
}
