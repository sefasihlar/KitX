using NLayer.Core.Concreate;
using NLayer.Core.DTOs.ProductDtos;
using NLayer.Core.DTOs.UserDtos;

namespace NLayer.Core.DTOs.FeatureProductUserDtos
{
    public class AnimalFeatureUserDto : AnimalProductFeature
    {
        public ProductDto Product { get; set; }
        public AppUserDto User { get; set; }
    }
}
