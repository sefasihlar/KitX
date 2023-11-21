using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : CustomBaseController
    {
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public LogsController(ILogService logService, IMapper mapper)
        {
            _logService = logService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await _logService.GetAllAsycn();
            var valuesConvert = _mapper.Map<List<Log>>(values);
            return CreateActionResult(CustomResponseDto<List<Log>>.Success(200, valuesConvert));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllLogs()
        {
            var values =await _logService.GetAllAsycn();


            await _logService.RemoveRangeAsycn(values);
          
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }

}
