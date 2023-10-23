using NLayer.Core.Concreate;

namespace NLayer.Core.Abstract
{
    public abstract class BaseFeatureProduct
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


        public bool? Condition { get; set; }
    }
}
