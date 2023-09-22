using NLayer.Core.Abstract;
using NLayer.Core.Concreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs.AnimalPhotoDtos
{
    public class AnimalPhotoDto:BaseDto
    {
        public string? ImageUrl { get; set; }
        public int AnimalId { get; set; }
        public bool Condition { get; set; }
    }
}
