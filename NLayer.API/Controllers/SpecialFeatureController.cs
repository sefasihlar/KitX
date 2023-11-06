using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.FeatureWithUserDtos.PersonProductFeatureDtos;
using NLayer.Core.DTOs.FeatureWithUserDtos.SpecialProductFeatureDtos;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
  
    [EnableCors("AllowMyOrigin")]
    [Authorize(AuthenticationSchemes = "Roles")]
    [Route("api/[controller]")]
    [ApiController]


    public class SpecialFeatureController : CustomBaseController
    {
        private readonly ISpecialProductFeatureService _specialProductFeatureService;
        private readonly IMapper _mapper;

        public SpecialFeatureController(ISpecialProductFeatureService specialProductFeatureService, IMapper mapper)
        {
            _specialProductFeatureService=specialProductFeatureService;
            _mapper=mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByIdWithProduct(int productId)
        {
            var values = await _specialProductFeatureService.FindByProductIdAsync(productId);
            var valesDto = _mapper.Map<SpecialFeatureProductUser>(values);
            return CreateActionResult(CustomResponseDto<SpecialFeatureProductUser>.Success(200, (valesDto)));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateProperty(SpecialProductFeatureDto dto)
        {
            var val = await _specialProductFeatureService.FindByProductIdAsync(dto.ProductId);
            val.Condition = true;

            val.Instegram = dto.Instegram;
            val.Twitter = dto.Twitter;
            val.Facebook = dto.Facebook;
            val.Text1 = dto.Text1;
            val.Text2 = dto.Text2;

            val.UpdatedDate =DateTime.Now;

            await _specialProductFeatureService.UpdateAsycn(val);

            //var userProductValues = new UserProductDto()
            //{
            //    UserId = dto.UserId,
            //    ProductId = dto.ProductId,
            //};

            ////daha önce bir ilişki oluşturlmuşmu
            //var userproductsin = await _userProductService.GetByIdsAsycn(dto.UserId, dto.ProductId);

            //if (userproductsin==null)
            //{
            //    await _userProductService.AddAsycn(_mapper.Map<UserProduct>(userProductValues));
            //}

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var values = await _specialProductFeatureService.GetAllAsycn();
            var valesDto = _mapper.Map<List<SpecialProductFeatureDto>>(values);
            return CreateActionResult(CustomResponseDto<List<SpecialProductFeatureDto>>.Success(200, valesDto));
        }
    }
}
