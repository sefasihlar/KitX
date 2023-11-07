using NLayer.Core.DTOs.FeatureDtos.AnimalProductFeatureDtos;
using NLayer.Core.DTOs.FeatureWithUserDtos.BelongingProductFeatureDtos;
using NLayer.Core.DTOs.FeatureWithUserDtos.PersonProductFeatureDtos;
using NLayer.Core.DTOs.FeatureWithUserDtos.SpecialProductFeatureDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs.ProductDtos
{
    public class GetWithCategoryListProductDto
    {
        public List<AnimalProductFeatureDto> Animals { get; set; }
        public List<PersonProductFeatureDto> Persons { get; set; }
        public List<BelongingProductFeatureDto> Belonging { get; set; }
        public List<SpecialProductFeatureDto> Specials { get; set; }
    }
}
