using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.LocationDtos;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{


    [Route("api/[controller]")]
    [ApiController]

    public class LocationController : CustomBaseController
    {
        private readonly ILocationservicecs _locationservicecs;
        private readonly IMapper _mapper;

        public LocationController(ILocationservicecs locationservicecs, IMapper mapper)
        {
            _locationservicecs=locationservicecs;
            _mapper=mapper;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Create(LocationDto dto)
        {

            var values = _mapper.Map<Location>(dto);
            if (dto != null)
            {
                dto.CreatedDate = DateTime.Now;
                
                await _locationservicecs.AddAsycn(values);

                return CreateActionResult(CustomResponseDto<LocationDto>.Success(201, dto));

            }

            return CreateActionResult(CustomResponseDto<LocationDto>.Success(201, dto));
        }


        [HttpGet("[action]/{productId}")]
        public async Task<IActionResult> GetUserLocations(int productId)
        {
            var values = await _locationservicecs.GetUserLocations(productId);
            if (values == null) {


                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Kullanıcıya ait location verisi bulunamadı"));
            }
            var valuesDto = _mapper.Map<List<LocationDto>>(values);

            return CreateActionResult(CustomResponseDto<List<LocationDto>>.Success(200, valuesDto));
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var values = await _locationservicecs.GetAllAsycn();
            var valuesDto = _mapper.Map<List<LocationDto>>(values);
            return CreateActionResult(CustomResponseDto<List<LocationDto>>.Success(200, valuesDto));
        }
    }
}
