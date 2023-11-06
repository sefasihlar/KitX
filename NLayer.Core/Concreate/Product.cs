using NLayer.Core.Abstract;
using System.Runtime.CompilerServices;

namespace NLayer.Core.Concreate
{
    public class Product : BaseProduct
    {

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public bool Status { get; set; }

        public List<UserProduct> UserProduct { get; set; }

        public List<ProductPhoto>? ProductPhotos { get; set; }
        public AnimalProductFeature? AnimalProductFeature { get; set; }
        public BelongingProductFeature? BelongingProductFeature { get; set; }
        public PersonProductFeature? PersonProductFeature { get; set; }
        public SpecialProductFeature? SpecialProductFeature { get; set; }
    }
}
