using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.AccountDtos;
using NLayer.Core.DTOs.EmailDtos;
using NLayer.Core.DTOs.TokenDtos;
using NLayer.Core.DTOs.UserDtos;
using NLayer.Core.Services;
using NLayer.Core.Token;
using NLayer.Service.Services;
using Org.BouncyCastle.Ocsp;

namespace NLayer.API.Controllers
{
    [EnableCors("AllowMyOrigin")]
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : CustomBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSenderService _emailSenderService;
        private readonly AppUserService _userService;
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenHandler _tokenHandler;

        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, IMapper mapper = null, ITokenHandler tokenHandler = null, AppUserService userService = null, IEmailSenderService emailSenderService = null)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenHandler = tokenHandler;
            _userService = userService;
            _emailSenderService = emailSenderService;
        }
        //[Authorize(AuthenticationSchemes = "Roles")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var usersDtos = _mapper.Map<List<UserListDto>>(users.ToList());

            await _emailSenderService.SendEmailAsync("sihlarsefa7@gmail.com", "konu açıklaması", "<h1>Test Email</h1>");

            return CreateActionResult(CustomResponseDto<List<UserListDto>>.Success(200, usersDtos));

        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetFindUser(int userId)
        {
            var user = await _userManager.FindByIdAsync(Convert.ToString(userId));
            if (user==null)
            {
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Kullanıcı bulunamadı"));
            }


            var userDto = _mapper.Map<AppUserDto>(user);

            return CreateActionResult(CustomResponseDto<AppUserDto>.Success(200, userDto));
        }


        //[Authorize(AuthenticationSchemes = "Roles")]
        [HttpGet("GetUserRole/{id}")]
        public async Task<IActionResult> GetUserRole(int id)
        {
            var user = await _userManager.FindByIdAsync(Convert.ToString(id));

            if (user==null)
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Kullanıcı bulunamadı"));

            var roles = await _userManager.GetRolesAsync(user);

            if (roles==null)
            {
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Kullanıcıya ait role bulunamadı"));
            }

            var usersDtos = _mapper.Map<List<AppUserRoleDto>>(roles.ToList());

            return CreateActionResult(CustomResponseDto<List<AppUserRoleDto>>.Success(200, usersDtos));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Savefile(IFormFile file, int userId)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    // Dosya adını kullanıcı ID'sini kullanarak oluşturduk.
                    var fileName = $"{userId}.png";

                    // Dosyayı wwwroot/UserImage klasörüne kaydedin.
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UserImage", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var user = await _userManager.FindByIdAsync(Convert.ToString(userId));

                    if (user != null)
                    {
                        user.ImageUrl = fileName;
                        await _userManager.UpdateAsync(user);
                        return CreateActionResult(CustomResponseDto<NoContentDto>.Success(201));
                    }
                    else
                    {
                        return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(404, "Kullanıcı bulunamadı."));
                    }
                }
                catch (Exception ex)
                {
                    return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(500, "Dosya kaydetme hatası: " + ex.Message));
                }
            }

            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Geçersiz dosya veya dosya yok."));
        }




        //[Authorize(AuthenticationSchemes = "Roles")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto appUserDto)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser()
                {
                    Email = appUserDto.Email,
                    PhoneNumber = appUserDto.Phone,
                    Name = appUserDto.Name,
                    Surname = appUserDto.Surname,
                    UserName = appUserDto.UserName,
                };

                var result = await _userManager.CreateAsync(user, appUserDto.Password);

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.ActionLink("ConfirmEmail", "Account", new
                    {
                        userId = user.Id,
                        Token = code

                    });

                    await _emailSenderService.SendEmailAsync(user.Email, "Hesabını Onayla", $"<!DOCTYPE html>\r\n<html>\r\n\r\n<head>\r\n<title></title>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\r\n    <style type=\"text/css\">\r\n        @media screen {{\r\n            @font-face {{\r\n                font-family: 'Lato';\r\n                font-style: normal;\r\n                font-weight: 400;\r\n                src: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');\r\n            }}\r\n\r\n            @font-face {{\r\n                font-family: 'Lato';\r\n                font-style: normal;\r\n                font-weight: 700;\r\n                src: local('Lato Bold'), local('Lato-Bold'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');\r\n            }}\r\n\r\n            @font-face {{\r\n                font-family: 'Lato';\r\n                font-style: italic;\r\n                font-weight: 400;\r\n src: local('Lato Italic'), local('Lato-Italic'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');\r\n            }}\r\n\r\n            @font-face {{\r\n                font-family: 'Lato';\r\n                font-style: italic;\r\n                font-weight: 700;\r\n                src: local('Lato Bold Italic'), local('Lato-BoldItalic'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');\r\n            }}\r\n        }}\r\n\r\n        /* CLIENT-SPECIFIC STYLES */\r\n        body,\r\n        table,\r\n        td,\r\n        a {{\r\n            -webkit-text-size-adjust: 100%;\r\n            -ms-text-size-adjust: 100%;\r\n        }}\r\n\r\n        table,\r\n        td {{\r\n            mso-table-lspace: 0pt;\r\n            mso-table-rspace: 0pt;\r\n        }}\r\n\r\n        img {{\r\n            -ms-interpolation-mode: bicubic;\r\n        }}\r\n\r\n        /* RESET STYLES */\r\n        img {{\r\n            border: 0;\r\n            height: auto;\r\n            line-height: 100%;\r\n            outline: none;\r\n            text-decoration: none;\r\n        }}\r\n\r\n        table {{\r\n            border-collapse: collapse !important;\r\n        }}\r\n\r\n        body {{\r\n            height: 100% !important;\r\n            margin: 0 !important;\r\n            padding: 0 !important;\r\n            width: 100% !important;\r\n        }}\r\n\r\n        /* iOS BLUE LINKS */\r\n        a[x-apple-data-detectors] {{\r\n            color: inherit !important;\r\n            text-decoration: none !important;\r\n            font-size: inherit !important;\r\n            font-family: inherit !important;\r\n            font-weight: inherit !important;\r\n            line-height: inherit !important;\r\n        }}\r\n\r\n        /* MOBILE STYLES */\r\n        @media screen and (max-width:600px) {{\r\n            h1 {{\r\n                font-size: 32px !important;\r\n                line-height: 32px !important;\r\n            }}\r\n        }}\r\n\r\n        /* ANDROID CENTER FIX */\r\n        div[style*=\"margin: 16px 0;\"] {{\r\n            margin: 0 !important;\r\n        }}\r\n    </style>\r\n</head>\r\n\r\n<body style=\"background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;\">\r\n    <!-- HIDDEN PREHEADER TEXT -->\r\n    <div\r\n        style=\"display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: 'Lato', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;\">\r\n        KitX&rsquo;e hoşgeldin! Seni bekliyorduk, hesabını aktifleştirmeyi unutma!\r\n    </div>\r\n    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n        <!-- LOGO -->\r\n        <tr>\r\n            <td bgcolor=\"#FFA73B\" align=\"center\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td align=\"center\" valign=\"top\" style=\"padding: 40px 10px 40px 10px;\"> </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#FFA73B\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"center\" valign=\"top\"\r\n                            style=\"padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;\">\r\n                            <img src=\"https://play-lh.googleusercontent.com/7yvZO3ms22hhnKGGzw8rn5EZChMJdUXhtFFqeTRVM9MmCDRl_MnraQHOX3PE7we9Uarp=w240-h480-rw\"\r\n                                width=\"125\" height=\"120\" style=\"display: block; border: 0px;\" />\r\n                            <h1 style=\"font-size: 48px; font-weight: 400; margin: 2;\">Aramıza Hoşgeldin!</h1>\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"left\"\r\n                            style=\"padding: 20px 30px 40px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n                            <p style=\"margin: 0;\">\r\n                                KitX&rsquo;e hoşgeldin! Hesabınızı aktifleştirmek için aşağıdaki butona tıklayın.\r\n                            </p>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"left\">\r\n                            <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n                                <tr>\r\n                                    <td bgcolor=\"#ffffff\" align=\"center\" style=\"padding: 20px 30px 60px 30px;\">\r\n                                        <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n                                            <tr>\r\n  <td align=\"center\" style=\"border-radius: 3px;\" bgcolor=\"#FFA73B\">\r\n<a href='{callbackUrl}' target=\"_blank\"\r\n                                                        style=\"font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #FFA73B; display: inline-block;\">\r\n                                                        Hesabımı Aktifleştir\r\n                                                    </a>\r\n                                                </td>\r\n                                            </tr>\r\n                                        </table>\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                        </td>\r\n                    </tr> <!-- COPY -->\r\n                    <tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"left\"\r\n                            style=\"padding: 0px 30px 0px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n                            <p style=\"margin: 0;\">Eğer butona tıklamakta sorun yaşıyorsanız, aşağıdaki linki\r\n                                tarayıcınızın\r\n                                adres çubuğuna kopyalayın ve enter tuşuna basın.\r\n                            </p>\r\n                        </td>\r\n                    </tr> <!-- COPY -->\r\n                    <tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"left\"\r\n                            style=\"padding: 20px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n                            <p style=\"margin: 0;\"><a href=\"#\" target=\"_blank\" style=\"color: #FFA73B;\">link</a></p>\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n    <div align=\"center\" style=\"margin-top:0px;margin-bottom:0px;padding:0px;\">\r\n    <div dir=\"ltr\" align=\"center\" style=\"margin-top:0px;margin-bottom:0px;padding:0px;background-color:#f4f4f4;max-width: 600px;\" width=\"100%\">\r\n        <div\r\n            style=\"color:rgb(0,0,0);font-family:'Times New Roman';font-size:medium;width:130px;max-width:130px;min-width:100px;padding-top:15px\">\r\n            <img src=\"https://play-lh.googleusercontent.com/7yvZO3ms22hhnKGGzw8rn5EZChMJdUXhtFFqeTRVM9MmCDRl_MnraQHOX3PE7we9Uarp=w240-h480-rw\"\r\n                style=\"margin-top: 1.4em;margin-left:1.1em;width:90px;\">\r\n        </div>\r\n        <div\r\n            style=\"width:190px;max-width:190px;font-family:'Lucida Grande',Tahoma;font-size:12px;margin-top:0.5em;color:rgb(102,102,102);letter-spacing:2px;padding-top:3px;padding-left:10px;overflow:hidden\">\r\n            <p>KitX&nbsp;<br></p>\r\n            <p>+90 555 555 55 55&nbsp;<br>\r\n                <a href=\"https://kitxapp.com/\"\r\n                    style=\"margin-top:0.5em;color:rgb(102,102,102);text-decoration:none\" target=\"_blank\">kitxapp.com</a>&nbsp;<br>\r\n            </p>\r\n            <p>\r\n                <a href=\"https://kitxapp.com\"\r\n                    style=\"margin-top:0.5em;color:rgb(102,102,102);text-decoration:none\" target=\"_blank\">\r\n                    <img\r\n                        src=\"https://i.imgur.com/9srAeBF.png\">\r\n                </a>&nbsp;\r\n                <a href=\"https://kitxapp.com\"\r\n                    style=\"margin-top:0.5em;color:rgb(102,102,102);text-decoration:none\" target=\"_blank\">\r\n                    <img\r\n                        src=\"https://i.imgur.com/E3YLJLI.png\">\r\n                </a>&nbsp;\r\n                <a href=\"https://kitxapp.com\"\r\n                    style=\"margin-top:0.5em;color:rgb(102,102,102);text-decoration:none\" target=\"_blank\">\r\n                    <img\r\n                        src=\"https://i.imgur.com/y6LiHYh.png\">\r\n                </a>\r\n            </p>\r\n        </div>\r\n        <div\r\n            style=\"width:190px;max-width:190px;font-family:'Lucida Grande',Tahoma;font-size:12px;margin-top:0.5em;color:rgb(102,102,102);letter-spacing:2px;border-left-width:2px;border-left-style:solid;border-left-color:rgb(251,224,181);padding-top:3px;padding-left:10px;overflow:hidden\">\r\n        </div>\r\n    </div>\r\n    </div>\r\n\r\n</body>\r\n\r\n</html>");
                    return CreateActionResult(CustomResponseDto<NoContentDto>.Success(200));

                }

                return CreateActionResult(CustomResponseDto<UserRegisterDto>.Success(201, appUserDto));

            }

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var turkishErrors = errors.Select(error => TranslateToTurkish(error)).ToList();
            var errorMessage = "Geçersiz giriş:\n" + string.Join("\n", turkishErrors);

            return CreateActionResult(CustomResponseDto<UserRegisterDto>.Fail(400, errorMessage));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Eposta onaylama işlemli başarısız"));
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return CreateActionResult(CustomResponseDto<NoContentDto>.Success(200));
                }
            }

            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Eposta onaylama işlemli başarısız"));
        }







        private string TranslateToTurkish(string englishMessage)
        {
            // Hata mesajlarını Türkçeye çevirin
            // Örnek: "Passwords must match." -> "Şifreler uyuşmalıdır."

            // Bu metodu kullanarak hata mesajlarını çevirebilirsiniz.
            // Örneğin:
            if (englishMessage == "Passwords must match.")
            {
                return "Şifreler uyuşmalıdır.";
            }
            // Diğer hata mesajlarını da benzer şekilde çevirebilirsiniz.

            // Çevirilmemiş hata mesajlarını olduğu gibi döndürün
            return englishMessage;
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
                    // Kullanıcı adı bulunamadı, kullanıcı adı yanlış
                    return BadRequest("Kullanıcı adı bulunamadı");
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
                else
                {
                    // Kimlik doğrulama başarısız, şifre yanlış
                    return BadRequest("Şifre yanlış");
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
