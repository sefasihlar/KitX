using NLayer.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Concreate
{
    public class AnimalPhoto:BaseEntity
    {
        public string? ImageUrl { get; set; }
        public int AnimalId { get; set; }
        public Animal? Animal { get; set; }
    }
}
