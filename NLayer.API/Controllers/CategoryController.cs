using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.CategoryDtos;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
  
    [EnableCors("AllowMyOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService=categoryService;
            _mapper=mapper;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var values = await _categoryService.GetAllAsycn();
            var valuesDto = _mapper.Map<List<CategoryDto>>(values);
            return CreateActionResult(CustomResponseDto<List<CategoryDto>>.Success(200, valuesDto));
        }
    }
}
