using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.CategoryDtos;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
  
    [EnableCors("AllowMyOrigin")]
    [Authorize(AuthenticationSchemes = "Roles")]
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


        [HttpGet("[action]")]
        public async Task<IActionResult> GetById(int id)
        {
            var valuesDto = new CategoryDto();

            var values = await _categoryService.GetByIdAsycn(id);
            if (values==null)
            {
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Categori bulunmadı"));
            }

            var categoryMapper = _mapper.Map<CategoryDto>(values);


            return CreateActionResult(CustomResponseDto<CategoryDto>.Success(200, categoryMapper));


           
        }

        [HttpPost("[action]")]

        public async Task<IActionResult> Create(CategoryDto dto)
        {
            if (dto!=null)
            {
                var valuesEntity = _mapper.Map<Category>(dto);

                var values = await _categoryService.AddAsycn(valuesEntity);

                return CreateActionResult(CustomResponseDto<CategoryDto>.Success(200, dto));
                    
              
            }

            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Category ekleme işlemi başarısız"));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update(CategoryDto dto)
        {
            var values = await _categoryService.GetByIdAsycn(dto.Id);
            if (values!=null)
            {
                values.Name = dto.Name;
                values.Condition = dto.Condition;
                values.CreatedDate = DateTime.Now;


                return CreateActionResult(CustomResponseDto<CategoryDto>.Success(200, dto));
            }

            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Category güncelleme işlememi başarısız"));
        }

        [HttpPost("[action]")]
         public async Task<IActionResult> Delete(int id)
        {
            var values = await _categoryService.GetByIdAsycn(id);
            if (values!=null)
            {
                await _categoryService.RemoveAsycn(values);

                return CreateActionResult(CustomResponseDto<NoContentDto>.Success(200));
            }

            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Category silme işlemi başarısız oldu Lütfen daha sonra tekarar deneyiniz"));
        }
            

     
    }
}
