namespace NLayer.Core.Abstract
{
    public class BaseFeatureProductDto
    {
        public int Id { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


        public bool? Condition { get; set; }
    }
}
