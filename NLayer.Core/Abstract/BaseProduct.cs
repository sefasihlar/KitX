namespace NLayer.Core.Abstract
{
    public abstract class BaseProduct
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool? Condition { get; set; }
    }
}
