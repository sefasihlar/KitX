using NLayer.Core.Abstract;

namespace NLayer.Core.DTOs.FeatureWithUserDtos.PersonProductFeatureDtos
{
    public class PersonProductFeatureDto : BaseFeatureProductDto
    {
        public string? Name { get; set; }
        public string? Property { get; set; }
        public string? Address { get; set; }
        public string? Tall { get; set; }
        public int Weight { get; set; }
        public string? HairColor { get; set; }
        public string? EyeColor { get; set; }
        public string? SkinColor { get; set; }

        public string? Instegram { get; set; }
        public string? Twitter { get; set; }
        public string? Facebook { get; set; }
        public string? Text1 { get; set; }
        public string? Text2 { get; set; }

        public int ProductId { get; set; }
    }
}
