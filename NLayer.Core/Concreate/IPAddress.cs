using NLayer.Core.Abstract;

namespace NLayer.Core.Concreate
{
    public class IPAddress : BaseEntity
    {
        public string? IPAdress { get; set; }
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
        public int ProductId { get; set; }
        public QrCode Product { get; set; }

    }
}
