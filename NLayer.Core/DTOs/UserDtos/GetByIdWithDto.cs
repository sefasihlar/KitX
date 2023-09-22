using NLayer.Core.DTOs.AnimalDtos;
using NLayer.Core.DTOs.ProductDtos;

namespace NLayer.Core.DTOs.UserDtos
{
    public class GetByIdWithDto : AppUserDto
    {
        public AnimalDto Animal { get; set; }
        public ProductDto Product { get; set; }
    }
}
