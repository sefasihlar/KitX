using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.RoleDtos;

namespace NLayer.API.Controllers
{

    //[Authorize(AuthenticationSchemes = "Roles")]
   
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RoleController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager, IMapper mapper, UserManager<AppUser> userManager, ILogger<RoleController> logger)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
            _logger=logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var roles = _roleManager.Roles.ToList();
            var RoleDtos = _mapper.Map<List<AppRoleDto>>(roles.ToList());
            return CreateActionResult(CustomResponseDto<List<AppRoleDto>>.Success(200, RoleDtos));
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppRoleDto appRoleDto)
        {
            var AppRole = await _roleManager.CreateAsync(_mapper.Map<AppRole>(appRoleDto));
            if (AppRole.Succeeded)
            {
                return CreateActionResult(CustomResponseDto<AppRoleDto>.Success(201, appRoleDto));
            }

            return CreateActionResult(CustomResponseDto<AppRoleDto>.Fail(400, "Kayıt oluşturma basarız"));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Role = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
            var RoleDto = _mapper.Map<AppRoleDto>(Role);
            return CreateActionResult(CustomResponseDto<AppRoleDto>.Success(200, RoleDto));

        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateRoleDto appRoleDto)
        {
            var appRole = _mapper.Map<AppRole>(appRoleDto);

            try
            {
                var result = await _roleManager.UpdateAsync(appRole);

                if (result.Succeeded)
                {
                    return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
                }
                else
                {
                    // Hata mesajlarını loglama işlemi
                    foreach (var error in result.Errors)
                    {
                        // Hata mesajını loglama mekanizmanızı kullanarak kaydedin
                        _logger.LogError($"Hata Kodu: {error.Code}, Hata Mesajı: {error.Description}");
                    }

                    // Kullanıcıya daha açıklayıcı bir hata mesajı dön
                    var errorMessage = "Güncelleme işlemi başarısız. Lütfen daha sonra tekrar deneyin.";
                    var errorDescriptions = result.Errors.Select(e => e.Description).ToList();

                    // Hata ayrıntılarını kullanıcıya göster
                    if (errorDescriptions.Any())
                    {
                        errorMessage += " Hata Detayları: " + string.Join(", ", errorDescriptions);
                    }

                    return CreateActionResult(CustomResponseDto<UpdateRoleDto>.Fail(400, errorMessage));
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Eş zamanlılık hatasını yakalayın ve kullanıcıya açıklayıcı bir mesaj döndürün
                _logger.LogError("Eş Zamanlılık Hatası: " + ex.Message);
                return CreateActionResult(CustomResponseDto<UpdateRoleDto>.Fail(409, "Veri başkası tarafından güncellendi. Lütfen daha sonra tekrar deneyin."));
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = _roleManager.Roles.FirstOrDefault(x => x.Id == id);


            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
            }

            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Role silme işlemi başarısız"));
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(RoleAssingDto roleAssingDto)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == roleAssingDto.UserId);
            var userValue = _mapper.Map<AppUser>(user);

            var role = _roleManager.Roles.FirstOrDefault(x => x.Id == roleAssingDto.RoleId);
            var roleValue = _mapper.Map<AppRole>(role);




            if (user != null && role != null)
            {
                var result = await _userManager.AddToRoleAsync(userValue, roleValue.Name);
                if (result.Succeeded)
                {
                    return CreateActionResult(CustomResponseDto<RoleAssingDto>.Success(201, roleAssingDto));
                }
            }

            return BadRequest("Role atama işlemi başarısız.");
        }


        [HttpPost("DeleteUserRole")]
        public async Task<IActionResult> DeleteUserRole(RoleAssingDto roleAssingDto)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == roleAssingDto.UserId);
            var userValue = _mapper.Map<AppUser>(user);

            var role = _roleManager.Roles.FirstOrDefault(x => x.Id == roleAssingDto.RoleId);
            var roleValue = _mapper.Map<AppRole>(role);




            if (user != null && role != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(userValue, roleValue.Name);
                if (result.Succeeded)
                {
                    return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
                }
            }

            return BadRequest("Role silme işlemi başarısız.");
        }



    }
}
