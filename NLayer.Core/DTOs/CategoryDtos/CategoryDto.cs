﻿using NLayer.Core.Abstract;

namespace NLayer.Core.DTOs.CategoryDtos
{
    public class CategoryDto : BaseDto
    {
        public string? Name { get; set; }
        public bool Condition { get; set; }
    }
}
