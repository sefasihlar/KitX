using NLayer.Core.Abstract;
using NLayer.Core.DTOs.CategoryDtos;

namespace NLayer.Core.DTOs.ProductDtos
{
    public class ProductDto : BaseDto
    {

        public int CategoryId { get; set; }
        public CategoryDto Category { get; set; }

        public bool Condition { get; set; }


    }
}
