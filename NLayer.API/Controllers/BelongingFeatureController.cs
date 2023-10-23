using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.FeatureWithUserDtos.BelongingProductFeatureDtos;
using NLayer.Core.DTOs.FeatureWithUserDtos.SpecialProductFeatureDtos;
using NLayer.Core.Services;
using NLayer.Service.Services;

namespace NLayer.API.Controllers
{
  
    [EnableCors("AllowMyOrigin")]
    [Route("api/[controller]")]
    [ApiController]

    public class BelongingFeatureController : CustomBaseController
    {
        private readonly IBelongingProductFeatureService _productFeatureService;
        private readonly IMapper _mapper;

        public BelongingFeatureController(IBelongingProductFeatureService productFeatureService, IMapper mapper)
        {
            _productFeatureService=productFeatureService;
            _mapper=mapper;
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetByIdWithProduct(int productId)
        {
            var values = await _productFeatureService.FindByProductIdAsync(productId);
            var valesDto = _mapper.Map<BelongingFeatureUserPorudct>(values);
            return CreateActionResult(CustomResponseDto<BelongingFeatureUserPorudct>.Success(200, (valesDto)));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateProperty(BelongingProductFeatureDto dto)
        {
            var val = await _productFeatureService.FindByProductIdAsync(dto.ProductId);
            val.Condition = true;

            val.Instegram = dto.Instegram;
            val.Twitter = dto.Twitter;
            val.Facebook = dto.Facebook;
            val.Text1 = dto.Text1;
            val.Text2 = dto.Text2;

            await _productFeatureService.UpdateAsycn(val);

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
            var values = await _productFeatureService.GetAllAsycn();
            var valesDto = _mapper.Map<List<BelongingProductFeatureDto>>(values);
            return CreateActionResult(CustomResponseDto<List<BelongingProductFeatureDto>>.Success(200, valesDto));
        }
    }
}
