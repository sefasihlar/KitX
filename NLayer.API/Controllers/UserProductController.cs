using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.Repositories;

namespace NLayer.API.Controllers
{
    [EnableCors("AllowMyOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProductController : CustomBaseController
    {
        private readonly IUserProductRepository _userProductRepository;
        private readonly IMapper _mapper;

        public UserProductController(IUserProductRepository userProductRepository, IMapper mapper)
        {
            _userProductRepository=userProductRepository;
            _mapper=mapper;
        }


        //[HttpPost("[action]")]
        //public async Task<IActionResult> AddToProduct(UserProductDto dto)
        //{
        //    var values =  _userProductRepository.AddAsycn(_mapper.Map<UserProduct>(dto));
        //    var valuesDto = _mapper.Map<UserProductDto>(values);
        //    return CreateActionResult(CustomResponseDto<UserProductDto>.Success(201,valuesDto));
        //}
    }
}
