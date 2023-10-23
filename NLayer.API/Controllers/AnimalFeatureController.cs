using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.FeatureDtos.AnimalProductFeatureDtos;
using NLayer.Core.DTOs.FeatureWithUserDtos.AnimalProductFeatureDtos;
using NLayer.Core.Services;
using Org.BouncyCastle.Tsp;

namespace NLayer.API.Controllers
{
    
    [EnableCors("AllowMyOrigin")]
    [Route("api/[controller]")]
    [ApiController]

    public class AnimalFeatureController : CustomBaseController
    {
        private readonly IAnimalProductFeatureService _productFeatureService;
        private readonly IMapper _mapper;

        public AnimalFeatureController(IAnimalProductFeatureService productFeatureService, IMapper mapper)
        {
            _productFeatureService=productFeatureService;
            _mapper=mapper;
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetByIdWithProduct(int productId)
        {
            var values = await _productFeatureService.FindByProductIdAsync(productId);
            var valesDto = _mapper.Map<AnimalFeatureProductUserDto>(values);
            return CreateActionResult(CustomResponseDto<AnimalFeatureProductUserDto>.Success(200, (valesDto)));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateAnimalProperty(AnimalProductFeatureDto dto)
        {
            var animal = await _productFeatureService.GetByIdAsycn(dto.Id);
            animal.Condition = true;


            animal.Name = dto.Name;
            animal.Age = dto.Age;
            animal.Color = dto.Color;
            animal.UpdatedDate = DateTime.Now;
            animal.Condition = true;
            animal.Birthday = dto.Birthday;
            animal.DiseaseInformation = dto.DiseaseInformation;
            animal.DrugInformation = dto.DrugInformation;
            animal.PassportNumber = dto.PassportNumber;
            animal.Race = dto.Race;
            animal.Type = dto.Type;
            animal.Address1 = dto.Address1;
            animal.Address2 = dto.Address2;
            animal.UpdatedDate = DateTime.Now;
            animal.VaccineInformation = dto.VaccineInformation;
            await _productFeatureService.UpdateAsycn(animal);

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
            var valesDto = _mapper.Map<List<AnimalProductFeatureDto>>(values);
            return CreateActionResult(CustomResponseDto<List<AnimalProductFeatureDto>>.Success(200,valesDto));
        }
    }
}
