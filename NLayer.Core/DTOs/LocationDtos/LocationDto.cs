using NLayer.Core.Abstract;

namespace NLayer.Core.DTOs.LocationDtos
{
    public class LocationDto : BaseDto
    {
        public string? Latitude { get; set; }
        public string? Lonqitude { get; set; }
        public int ProductId { get; set; }
    }
}
