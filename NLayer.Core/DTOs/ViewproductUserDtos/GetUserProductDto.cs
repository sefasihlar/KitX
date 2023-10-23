using NLayer.Core.DTOs.ProductDtos;
using NLayer.Core.DTOs.UserDtos;

namespace NLayer.Core.DTOs.ViewproductUserDtos
{
    public class GetUserProductDto
    {
        public AppUserDto UserDto { get; set; }
        public ProductDto ProductDto { get; set; }

    }
}
