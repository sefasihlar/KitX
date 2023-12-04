using NLayer.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs.VersionDtos
{
    public class VersionDto:BaseDto
    {
        public string? Version { get; set; }
        public string? VersionCode { get; set; }
        public string? Description { get; set; }
        public bool Conditon { get; set; }
    }
}
