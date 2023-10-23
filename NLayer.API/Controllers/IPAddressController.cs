using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.IPAddressDtos;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Roles")]
    [EnableCors("AllowMyOrigin")]
    [Route("api/[controller]")]
    [ApiController]

    public class IPAddressController : CustomBaseController
    {
        private readonly IIPAddressService _ipAddressService;
        private readonly IMapper _mapper;


        public IPAddressController(IIPAddressService ipAddressService, IMapper mapper)
        {
            _ipAddressService=ipAddressService;
            _mapper=mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await _ipAddressService.GetWithProductListAsync();
            var valuesDto = _mapper.Map<List<IPAddressDto>>(values);
            return CreateActionResult(CustomResponseDto<List<IPAddressDto>>.Success(200, valuesDto));

        }
        //[HttpGet("[action]")]
        //public async Task<IActionResult> GetByProductId(int productId)
        //{

        //}
        [HttpGet("[action]/{productId}")]
        public async Task<IActionResult> GetIPAddress(int productId)
        {
            var values = await _ipAddressService.GetByIpAddressWithProductId(productId);
            var valuesDto = _mapper.Map<List<IPAddressDto>>(values);

            return CreateActionResult(CustomResponseDto<List<IPAddressDto>>.Success(200, valuesDto));
        }

    }
}
