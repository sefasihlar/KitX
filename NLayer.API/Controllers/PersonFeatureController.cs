using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.FeatureDtos.AnimalProductFeatureDtos;
using NLayer.Core.DTOs.FeatureWithUserDtos.PersonProductFeatureDtos;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]

    public class PersonFeatureController : CustomBaseController
    {
        private readonly IPersonelProductFeatureService _personelProductFeatureService;
        private readonly IMapper _mapper;

        public PersonFeatureController(IPersonelProductFeatureService personelProductFeatureService, IMapper mapper)
        {
            _personelProductFeatureService=personelProductFeatureService;
            _mapper=mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByIdWithProduct(int productId)
        {
            var values = await _personelProductFeatureService.FindByProductIdAsync(productId);
            var valesDto = _mapper.Map<PersonProductFeatureDto>(values);
            return CreateActionResult(CustomResponseDto<PersonProductFeatureDto>.Success(200, (valesDto)));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateProperty(PersonProductFeatureDto dto)
        {

            var val = await _personelProductFeatureService.FindByProductIdAsync(dto.ProductId);
            val.Condition = true;

            val.Name = dto.Name;
            val.Property = dto.Property;
            val.Address = dto.Address;
            val.Tall = dto.Tall;
            val.Weight = dto.Weight;
            val.HairColor = dto.HairColor;
            val.EyeColor = dto.EyeColor;
            val.SkinColor = dto.SkinColor;
            val.UpdatedDate = DateTime.Now;

            val.Instegram = dto.Instegram;
            val.Twitter = dto.Twitter;
            val.Facebook = dto.Facebook;
            val.Text1 = dto.Text1;
            val.Text2 = dto.Text2;

            await _personelProductFeatureService.UpdateAsycn(val);

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
            var values = await _personelProductFeatureService.GetAllAsycn();
            var valesDto = _mapper.Map<List<PersonProductFeatureDto>>(values);
            return CreateActionResult(CustomResponseDto<List<PersonProductFeatureDto>>.Success(200, valesDto));
        }
    }
}
