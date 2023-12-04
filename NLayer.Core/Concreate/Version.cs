using NLayer.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Concreate
{
    public class Version:BaseEntity
    {
        public string? version { get; set; }
        public string? VersionCode { get; set; }
        public string? Description { get; set; }
    }
}
