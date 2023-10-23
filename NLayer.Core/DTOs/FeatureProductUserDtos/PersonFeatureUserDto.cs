using NLayer.Core.DTOs.FeatureWithUserDtos.PersonProductFeatureDtos;
using NLayer.Core.DTOs.ProductDtos;
using NLayer.Core.DTOs.UserDtos;

namespace NLayer.Core.DTOs.FeatureProductUserDtos
{
    public class PersonFeatureUserDto : PersonProductFeatureDto
    {
        public ProductDto Product { get; set; }
        public AppUserDto User { get; set; }
    }
}
