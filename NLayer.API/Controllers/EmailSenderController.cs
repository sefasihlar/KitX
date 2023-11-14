using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.EmailDtos;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [EnableCors("AllowMyOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSenderController : CustomBaseController
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly IMapper _mapper;

        public EmailSenderController(IEmailSenderService emailSenderService, IMapper mapper)
        {
            _emailSenderService = emailSenderService;
            _mapper = mapper;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> SendEmail(SendEmailDto dto)
        {
            
           await _emailSenderService.SendEmailAsync(dto.To, dto.Subject,"");
           return CreateActionResult(CustomResponseDto<SendEmailDto>.Success(200,dto));

        }
    }
}
