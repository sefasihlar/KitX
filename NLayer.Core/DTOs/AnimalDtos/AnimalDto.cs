using NLayer.Core.Abstract;

namespace NLayer.Core.DTOs.AnimalDtos
{
    public class AnimalDto : BaseDto
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
        public string? ImageUrl { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public string? Color { get; set; }
    }
}
