using NLayer.Core.Abstract;

namespace NLayer.Core.DTOs.FeatureDtos.AnimalProductFeatureDtos
{
    public class AnimalProductFeatureDto : BaseFeatureProductDto
    {
        public string? PassportNumber { get; set; }
        public string? Race { get; set; }
        public string? Type { get; set; }
        public DateTime? Birthday { get; set; }
        public string? VaccineInformation { get; set; }
        public string? DiseaseInformation { get; set; }
        public string? DrugInformation { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Name { get; set; }
        public double? Age { get; set; }
        public double? Kg { get; set; }
        public string? Color { get; set; }

        public string? Instegram { get; set; }
        public string? Twitter { get; set; }
        public string? Facebook { get; set; }
        public string? Text1 { get; set; }
        public string? Text2 { get; set; }

        public int ProductId { get; set; }
    }
}
