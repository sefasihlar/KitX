using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.AnimalDtos;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Roles")]
    [EnableCors("AllowMyOrigin")]
    [Route("api/[controller]")]
    [ApiController]

    public class AnimalController : CustomBaseController
    {
        private readonly IAnimalService _animalService;
        private readonly IMapper _mapper;

        public AnimalController(IAnimalService animalService, IMapper mapper)
        {
            _animalService=animalService;
            _mapper=mapper;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var animlas = await _animalService.GetAllAsycn();
        //    var animalDto = _mapper.Map<List<AnimalDto>>(animlas);
        //    return CreateActionResult(CustomResponseDto<List<AnimalDto>>.Success(200, animalDto));
        //}

        //[HttpPost()]
        //public async Task<IActionResult> Create(AnimalDto anmimalDto)
        //{
        //    anmimalDto.CreatedDate = DateTime.Now;
        //    var Animal = await _animalService.AddAsycn(_mapper.Map<Product>(anmimalDto));
        //    var AnimalsDto = _mapper.Map<AnimalDto>(Animal);
        //    return CreateActionResult(CustomResponseDto<AnimalDto>.Success(201, AnimalsDto));

        //}

    }
}
