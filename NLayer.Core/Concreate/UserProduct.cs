namespace NLayer.Core.Concreate
{
    public class UserProduct
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}
