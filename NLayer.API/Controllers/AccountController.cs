using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.AccountDtos;
using NLayer.Core.DTOs.TokenDtos;
using NLayer.Core.DTOs.UserDtos;
using NLayer.Core.Token;

namespace NLayer.API.Controllers
{
    [EnableCors("AllowMyOrigin")]
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : CustomBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenHandler _tokenHandler;

        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, IMapper mapper = null, ITokenHandler tokenHandler = null)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenHandler = tokenHandler;
        }
        //[Authorize(AuthenticationSchemes = "Roles")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var usersDtos = _mapper.Map<List<UserListDto>>(users.ToList());

            return CreateActionResult(CustomResponseDto<List<UserListDto>>.Success(200, usersDtos));

        }
        //[Authorize(AuthenticationSchemes = "Roles")]
        [HttpGet("GetUserRole/{id}")]
        public async Task<IActionResult> GetUserRole(int id)
        {
            var user = await _userManager.FindByIdAsync(Convert.ToString(id));

            var roles = await _userManager.GetRolesAsync(user);

            var usersDtos = _mapper.Map<List<AppUserRoleDto>>(roles.ToList());

            return CreateActionResult(CustomResponseDto<List<AppUserRoleDto>>.Success(200, usersDtos));
        }


        //[Authorize(AuthenticationSchemes = "Roles")]
        [HttpPost("register")]

        public async Task<IActionResult> Register(UserRegisterDto appUserDto)
        {
            AppUser user = new AppUser()
            {

                Name = appUserDto.Name,
                Surname = appUserDto.Surname,
                UserName = appUserDto.UserName,

                PasswordHash = appUserDto.Password,

            };

            var result = await _userManager.CreateAsync(user, appUserDto.Password);

            if (result.Succeeded)
            {
                return CreateActionResult(CustomResponseDto<UserRegisterDto>.Success(201, appUserDto));
            }

            return CreateActionResult(CustomResponseDto<UserRegisterDto>.Fail(400, "Kayıt oluşturma basarız"));

        }

        //[HttpGet]
        //public async Task<IActionResult> Login(LoginDto loginDto)
        //{
        //    AppUser user = new AppUser()
        //    {
        //        UserName = loginDto.UserName,
        //        PasswordHash = loginDto.Password

        //    };

        //    return CreateActionResult(CustomResponseDto<LoginDto>.Success(201, loginDto));
        //}



        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginDto.UserName);
                if (user == null)
                {
                    return BadRequest(loginDto.UserName);
                }

                var userRoles = await _userManager.GetRolesAsync(user);

                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (result.Succeeded)
                {
                    var userId = _userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)User);

                    var TokenInfoDto = new TokenInfo()
                    {
                        UserId = user.Id,
                        Name = user.Name,
                        Surname = user.Surname,
                        Role = userRoles.FirstOrDefault()

                    };

                    TokenDto token = _tokenHandler.CreateAccessToken(TokenInfoDto);
                    token.Name = TokenInfoDto.Name;
                    token.Surname = TokenInfoDto.Surname;
                    token.Role = TokenInfoDto.Role;
                    token.UserId = TokenInfoDto.UserId;
                    return CreateActionResult(CustomResponseDto<TokenDto>.Success(200, token));
                }
            }

            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Giriş işlemi başarısız"));
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByNameAsync(resetPasswordDto.UserName);
            if (user == null)
            {
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Kullancı bulunamadı"));
            }

            var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, resetPasswordDto.Password);
            user.PasswordHash = newPasswordHash;

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                // Şifre sıfırlama başarılı.
                return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
            }
            else
            {
                // Şifre sıfırlama başarısız, hata işleme yapılabilir.
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Şifre değiştirme işlemi başarısız"));
            }
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            var user = await _userManager.FindByIdAsync(Convert.ToString(userUpdateDto.Id));

            if (user == null)
            {
                // Kullanıcı bulunamadı, hata işleme yapılabilir.
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Geçersiz kullanıcı"));
            }

            // Kullanıcı bilgilerini güncelleme
            user.Name = userUpdateDto.Name;
            user.Surname = userUpdateDto.SurName;
            user.UserName = userUpdateDto.UserName;
            user.Email = userUpdateDto.Email;
            user.PhoneNumber = userUpdateDto.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                // Güvenlik damgasını güncelleme
                await _userManager.UpdateSecurityStampAsync(user);

                return CreateActionResult(CustomResponseDto<AppUser>.Success(204, user));
            }
            else
            {
                // Güncelleme işlemi başarısız, hata işleme yapılabilir.
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Kullanıcı güncelleme işlemi başarısız"));
            }
        }




    }

}
