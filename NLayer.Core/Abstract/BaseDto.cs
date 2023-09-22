namespace NLayer.Core.Abstract
{
    public abstract class BaseDto
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;


    }
}
