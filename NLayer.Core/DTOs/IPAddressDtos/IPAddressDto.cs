using NLayer.Core.Abstract;
using NLayer.Core.Concreate;

namespace NLayer.Core.DTOs.IPAddressDtos
{
    public class IPAddressDto : BaseDto
    {
        public string? IPAdress { get; set; }
        public int ProductId { get; set; }
        public QrCode Product { get; set; }
        public bool Condition { get; set; }
        public bool IpHub { get; set; }

        public string Country_Code { get; set; }
        public string Country_Name { get; set; }
        public string Region_Name { get; set; }
        public string City_Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Zip_Code { get; set; }
        public string Time_Zone { get; set; }
        public string Asn { get; set; }
        public string As { get; set; }
        public bool is_proxy { get; set; }

    }
}
