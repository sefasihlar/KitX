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

    [EnableCors("AllowMyOrigin")]
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
                await _locationservicecs.AddAsycn(values);

                return CreateActionResult(CustomResponseDto<LocationDto>.Success(201, dto));

            }

            return CreateActionResult(CustomResponseDto<LocationDto>.Success(201, dto));
        }


        [HttpGet("[action]/{productId}")]
        public async Task<IActionResult> GetUserLocations(int productId)
        {
            var values = await _locationservicecs.GetUserLocations(productId);
            var valuesDto = _mapper.Map<List<LocationDto>>(values);

            return CreateActionResult(CustomResponseDto<List<LocationDto>>.Success(200, valuesDto));
        }

    }
}
