using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.VersionDtos;
using NLayer.Core.Services;
using Version = NLayer.Core.Concreate.Version;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : CustomBaseController
    {
       private readonly IVersionService _versionService;
        private readonly IMapper _mapper;

        public VersionController(IVersionService versionService, IMapper mapper)
        {
            _versionService = versionService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var values = await _versionService.GetAllAsycn();
            var valuesDto = _mapper.Map<List<VersionDto>>(values);
            return CreateActionResult(CustomResponseDto<List<VersionDto>>.Success(200, valuesDto));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddVersion(VersionDto dto)
        {
            var values = await _versionService.AddAsycn(_mapper.Map<Version>(dto));
            var valuesDto = _mapper.Map<VersionDto>(values);
            return CreateActionResult(CustomResponseDto<VersionDto>.Success(200, valuesDto));
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteVersion(int id)
        {
            var vales = await _versionService.GetByIdAsycn(id);
            if (vales!=null)
            {
                await _versionService.RemoveAsycn(vales);
                return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
            }

            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400,"Silme işlemi başarısız"));
        }
    }
}
