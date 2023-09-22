using NLayer.Core.DTOs.ProductDtos;
using NLayer.Core.DTOs.UserDtos;

namespace NLayer.Core.DTOs.UserProduct
{
    public class UserProductDto
    {
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int UserId { get; set; }
        public AppUserDto User { get; set; }
    }
}
