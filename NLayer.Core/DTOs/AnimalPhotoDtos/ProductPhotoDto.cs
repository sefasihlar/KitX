using NLayer.Core.Abstract;

namespace NLayer.Core.DTOs.AnimalPhotoDtos
{
    public class ProductPhotoDto : BaseDto
    {
        public string? ImageUrl { get; set; }
        public int ProductId { get; set; }
        public bool Condition { get; set; }
    }
}
