using NLayer.Core.Abstract;
using NLayer.Core.Concreate;

namespace NLayer.Core.DTOs.IPAddressDtos
{
    public class IPAddressDto : BaseDto
    {
        public string? IPAdress { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public bool Condition { get; set; }
        public bool IpHub { get; set; }
    }
}
