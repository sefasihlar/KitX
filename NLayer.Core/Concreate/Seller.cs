using NLayer.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Concreate
{
    public class Seller:BaseEntity
    {
        public string? Name { get; set; }
        public string? OwnerName { get; set; }
        public string? OwnerSurname { get; set; }
        public string? OwnerPhone { get; set; }
        public string? OwnerEmail { get; set;}
        public string? Address { get; set; }

    }
}
