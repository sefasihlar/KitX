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
        private Timer _timer;
        private readonly IMapper _mapper;
       
        private bool _isRunning;

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
            var values = await _logService.GetAllAsycn();


            await _logService.RemoveRangeAsycn(values);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }


        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteEmptyUserNameProperty()
        {
            var values = await _logService.GetAllAsycn();
            var valuesFilter = values.Where(x => x.UserName == null);
            if (valuesFilter !=null)
            {
                await _logService.RemoveRangeAsycn(valuesFilter);
            }

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
        

    }
}
