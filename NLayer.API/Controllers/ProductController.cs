using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.FeatureProductUserDtos;
using NLayer.Core.DTOs.IP2LocationsDtos;
using NLayer.Core.DTOs.IPAddressDtos;
using NLayer.Core.DTOs.ProductDtos;
using NLayer.Core.DTOs.QRCodeDtos;
using NLayer.Core.DTOs.UserDtos;
using NLayer.Core.DTOs.UserProduct;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Service.Services;

namespace NLayer.API.Controllers
{
 
    [EnableCors("AllowMyOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IProductService _productService;
        private readonly IAnimalService _animalService;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IUserProductService _userProductService;
        private readonly IAnimalPhotoService _animalPhotoService;
        private readonly IHubContext<IPHubService> _hubContext;
        private readonly ICategoryService _categoryService;
        private readonly IQRGeneratorService _qrGeneratorService;
        private readonly IQRCodeService _qrCodeService;
        private readonly IPersonelProductFeatureService _personelProductFeatureService;
        private readonly ISpecialProductFeatureService _specialProductFeatureService;
        private readonly IAnimalProductFeatureService _animalProductFeatureService;
        private readonly IBelongingProductFeatureService _belongingProductFeatureService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly HttpClient _httpClient;


        private readonly IMapper _mapper;


        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIPAddressService _ipAddressService;


        public ProductController(IProductService productService, IMapper mapper, IAnimalService animalService, IAppUserRepository appUserRepository, IUserProductService userProductService, IHttpContextAccessor httpContextAccessor, IIPAddressService ipAddressService, IAnimalPhotoService animalPhotoService, IIHubService hubService, IHubContext<IPHubService> hubContext, HttpClient httpClient, IQRGeneratorService qrGeneratorService, IQRCodeService qrCodeService, ICategoryService categoryService, IPersonelProductFeatureService personelProductFeatureService, ISpecialProductFeatureService specialProductFeatureService, IAnimalProductFeatureService animalProductFeatureService, IBelongingProductFeatureService belongingProductFeatureService, IEmailSenderService emailSenderService)
        {
            _productService = productService;
            _mapper = mapper;
            _animalService = animalService;
            _appUserRepository = appUserRepository;
            _userProductService = userProductService;
            _httpContextAccessor = httpContextAccessor;
            _ipAddressService = ipAddressService;
            _animalPhotoService = animalPhotoService;
            _hubContext = hubContext;
            _httpClient = httpClient;
            _qrGeneratorService = qrGeneratorService;
            _qrCodeService = qrCodeService;
            _categoryService = categoryService;
            _personelProductFeatureService = personelProductFeatureService;
            _specialProductFeatureService = specialProductFeatureService;
            _animalProductFeatureService = animalProductFeatureService;
            _belongingProductFeatureService = belongingProductFeatureService;
            _emailSenderService = emailSenderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var classes = await _productService.GetAllAsycn();
            var classDtos = _mapper.Map<List<ProductDto>>(classes);
            //return Ok( CustomResponseDto<List<ClassDto>>.Success(200, classDtos));
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, classDtos));

        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var values = await _productService.GetByIdAsycn(id);
            var valuesDto = _mapper.Map<ProductDto>(values);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, valuesDto));

        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetWithUserProduct(int id)
        {
            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();

            // X-Forwarded-For başlığını kontrol et
            var xForwardedForHeader = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrEmpty(xForwardedForHeader))
            {
                ipAddress = xForwardedForHeader.FirstOrDefault();
            }

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = "IP adresi bulunamadı.";
            }

            var product = await _productService.GetByUserProduct(id);
            if (product==null)
            {
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "böyle bir ürün bulunamadı"));
            }

            var userproduct = product.UserProduct.FirstOrDefault(x => x.ProductId == id);

            if (userproduct==null)
            {
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Kullancıya ait ürün bulunamadı.Bu ürün daha önce bir kullanıcıya atanmamış.Önce kullancıyla ilşki kurup sonra tekrar deneyiniz"));
            }

            var valuesDto = _mapper.Map<GetWithProductDto>(product);

            var valuesUserDto = userproduct.User;

            if (userproduct!=null)
            {
                var userId = userproduct.UserId;
                await _hubContext.Clients.All.SendAsync("QrCodeRead", $"IPAddress:{ipAddress}|ProdocutId:{id}|UserId:{userId}");
            }

            var apiUrl = $"https://api.ip2location.io/?key=E4042F9F8F99539DAD788952EE48B576&ip={ipAddress}&format=json";

            var response = await _httpClient.GetStringAsync(apiUrl);


            var ipLocationInfo = JsonConvert.DeserializeObject<IpLocationInfo>(response);


            var IPAddress = ipLocationInfo.ip;
            var countryCode = ipLocationInfo.country_code;
            var countryName = ipLocationInfo.country_name;


            var ipAdressValues = new IPAddressDto()
            {
                IPAdress = ipLocationInfo.ip,
                ProductId = id,
                Country_Code = ipLocationInfo.country_code,
                Country_Name = ipLocationInfo.country_name,
                Region_Name = ipLocationInfo.region_name,
                City_Name = ipLocationInfo.city_name,
                Latitude = Convert.ToString(ipLocationInfo.latitude),
                Longitude = Convert.ToString(ipLocationInfo.longitude),
                Zip_Code = ipLocationInfo.zip_code,
                Time_Zone = ipLocationInfo.time_zone,
                Asn = ipLocationInfo.asn,
                As = ipLocationInfo.As,
                is_proxy = ipLocationInfo.is_proxy,


            };

            if (valuesUserDto.Email !=null)
            {
                await _emailSenderService.SendEmailAsync(valuesUserDto.Email, "Kitiniz Bulundu", "<!DOCTYPE html>\r\n<html>\r\n\r\n<head>\r\n    <title></title>\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\r\n    <style type=\"text/css\">\r\n        @media screen {\r\n            @font-face {\r\n                font-family: 'Lato';\r\n                font-style: normal;\r\n                font-weight: 400;\r\n                src: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: 'Lato';\r\n                font-style: normal;\r\n                font-weight: 700;\r\n                src: local('Lato Bold'), local('Lato-Bold'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: 'Lato';\r\n                font-style: italic;\r\n                font-weight: 400;\r\n                src: local('Lato Italic'), local('Lato-Italic'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: 'Lato';\r\n                font-style: italic;\r\n                font-weight: 700;\r\n                src: local('Lato Bold Italic'), local('Lato-BoldItalic'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');\r\n            }\r\n        }\r\n\r\n        /* CLIENT-SPECIFIC STYLES */\r\n        body,\r\n        table,\r\n        td,\r\n        a {\r\n            -webkit-text-size-adjust: 100%;\r\n            -ms-text-size-adjust: 100%;\r\n        }\r\n\r\n        table,\r\n        td {\r\n            mso-table-lspace: 0pt;\r\n            mso-table-rspace: 0pt;\r\n        }\r\n\r\n        img {\r\n            -ms-interpolation-mode: bicubic;\r\n        }\r\n\r\n        /* RESET STYLES */\r\n        img {\r\n            border: 0;\r\n            height: auto;\r\n            line-height: 100%;\r\n            outline: none;\r\n            text-decoration: none;\r\n        }\r\n\r\n        table {\r\n            border-collapse: collapse !important;\r\n        }\r\n\r\n        body {\r\n            height: 100% !important;\r\n            margin: 0 !important;\r\n            padding: 0 !important;\r\n            width: 100% !important;\r\n        }\r\n\r\n        /* iOS BLUE LINKS */\r\n        a[x-apple-data-detectors] {\r\n            color: inherit !important;\r\n            text-decoration: none !important;\r\n            font-size: inherit !important;\r\n            font-family: inherit !important;\r\n            font-weight: inherit !important;\r\n            line-height: inherit !important;\r\n        }\r\n\r\n        /* MOBILE STYLES */\r\n        @media screen and (max-width:600px) {\r\n            h1 {\r\n                font-size: 32px !important;\r\n                line-height: 32px !important;\r\n            }\r\n        }\r\n\r\n        /* ANDROID CENTER FIX */\r\n        div[style*=\"margin: 16px 0;\"] {\r\n            margin: 0 !important;\r\n        }\r\n    </style>\r\n</head>\r\n\r\n<body style=\"background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;\">\r\n    <!-- HIDDEN PREHEADER TEXT -->\r\n    <div\r\n        style=\"display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: 'Lato', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;\">\r\n        Hey! Sana iyi haberlerimiz var! Kit&rsquo;iniz bulundu, şimdi hemen görebilirsin.\r\n    </div>\r\n    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n        <!-- LOGO -->\r\n        <tr>\r\n            <td bgcolor=\"#FFA73B\" align=\"center\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td align=\"center\" valign=\"top\" style=\"padding: 40px 10px 40px 10px;\"> </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#FFA73B\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"center\" valign=\"top\"\r\n                            style=\"padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;\">\r\n                            <img src=\"https://play-lh.googleusercontent.com/7yvZO3ms22hhnKGGzw8rn5EZChMJdUXhtFFqeTRVM9MmCDRl_MnraQHOX3PE7we9Uarp=w240-h480-rw\"\r\n                                width=\"125\" height=\"120\" style=\"display: block; border: 0px;\" />\r\n                            <h1 style=\"font-size: 48px; font-weight: 400; margin: 2;\">Kit&rsquo;iniz bulundu \U0001f973</h1>\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"left\"\r\n                            style=\"padding: 20px 30px 40px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n                            <p style=\"margin: 0;\">\r\n                                Hey! Sana iyi haberlerimiz var! Kit&rsquo;iniz bulundu, şimdi hemen görebilirsin.\r\n                                Aşağıdaki butona tıklayarak kit&rsquo;ini nerede bulduğumuzu görebilirsin.\r\n                            </p>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td bgcolor=\"#ffffff\" align=\"left\">\r\n                            <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n                                <tr>\r\n                                    <td bgcolor=\"#ffffff\" align=\"center\" style=\"padding: 20px 30px 60px 30px;\">\r\n                                        <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n                                            <tr>\r\n                                                <td align=\"center\" style=\"border-radius: 3px;\" bgcolor=\"#FFA73B\">\r\n                                                    <a href=\"#\" target=\"_blank\"\r\n                                                        style=\"font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #FFA73B; display: inline-block;\">\r\n                                                        Kit&rsquo;imi göster\r\n                                                    </a>\r\n                                                </td>\r\n                                            </tr>\r\n                                        </table>\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n    <div align=\"center\" style=\"margin-top:0px;margin-bottom:0px;padding:0px;\">\r\n        <div dir=\"ltr\" align=\"center\"\r\n            style=\"margin-top:0px;margin-bottom:0px;padding:0px;background-color:#f4f4f4;max-width: 600px;\"\r\n            width=\"100%\">\r\n            <div\r\n                style=\"color:rgb(0,0,0);font-family:'Times New Roman';font-size:medium;width:130px;max-width:130px;min-width:100px;padding-top:15px\">\r\n                <img src=\"https://play-lh.googleusercontent.com/7yvZO3ms22hhnKGGzw8rn5EZChMJdUXhtFFqeTRVM9MmCDRl_MnraQHOX3PE7we9Uarp=w240-h480-rw\"\r\n                    style=\"margin-top: 1.4em;margin-left:1.1em;width:90px;\">\r\n            </div>\r\n            <div\r\n                style=\"width:190px;max-width:190px;font-family:'Lucida Grande',Tahoma;font-size:12px;margin-top:0.5em;color:rgb(102,102,102);letter-spacing:2px;padding-top:3px;padding-left:10px;overflow:hidden\">\r\n                <p>KitX&nbsp;<br></p>\r\n                <p>+90 555 555 55 55&nbsp;<br>\r\n                    <a href=\"https://kitxapp.com/\" style=\"margin-top:0.5em;color:rgb(102,102,102);text-decoration:none\"\r\n                        target=\"_blank\">kitxapp.com</a>&nbsp;<br>\r\n                </p>\r\n                <p>\r\n                    <a href=\"https://kitxapp.com\" style=\"margin-top:0.5em;color:rgb(102,102,102);text-decoration:none\"\r\n                        target=\"_blank\">\r\n                        <img src=\"https://i.imgur.com/9srAeBF.png\">\r\n                    </a>&nbsp;\r\n                    <a href=\"https://kitxapp.com\" style=\"margin-top:0.5em;color:rgb(102,102,102);text-decoration:none\"\r\n                        target=\"_blank\">\r\n                        <img src=\"https://i.imgur.com/E3YLJLI.png\">\r\n                    </a>&nbsp;\r\n                    <a href=\"https://kitxapp.com\" style=\"margin-top:0.5em;color:rgb(102,102,102);text-decoration:none\"\r\n                        target=\"_blank\">\r\n                        <img src=\"https://i.imgur.com/y6LiHYh.png\">\r\n                    </a>\r\n                </p>\r\n            </div>\r\n            <div\r\n                style=\"width:190px;max-width:190px;font-family:'Lucida Grande',Tahoma;font-size:12px;margin-top:0.5em;color:rgb(102,102,102);letter-spacing:2px;border-left-width:2px;border-left-style:solid;border-left-color:rgb(251,224,181);padding-top:3px;padding-left:10px;overflow:hidden\">\r\n            </div>\r\n        </div>\r\n    </div>\r\n\r\n</body>\r\n\r\n</html>");
            }


            await _ipAddressService.AddAsycn(_mapper.Map<IPAddress>(ipAdressValues));

            var category = await _categoryService.GetByIdAsycn(product.CategoryId);


            if (category!=null)
            {
                if (category.Name == "Person")
                {
                    var productFeature = await _personelProductFeatureService.FindByProductIdAsync(id);
                    if (productFeature == null)
                    {
                        return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Not Find Product"));
                    }
                    var productDto = _mapper.Map<PersonFeatureUserDto>(productFeature);
                    productDto.User =_mapper.Map<AppUserDto>(valuesUserDto);

                    return CreateActionResult(CustomResponseDto<PersonFeatureUserDto>.Success(200, productDto));
                }


                else if (category.Name =="Animal")
                {
                    var productFeature = await _animalProductFeatureService.FindByProductIdAsync(id);
                    if (productFeature == null)
                    {
                        return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Not Find Product"));
                    }
                    var productDto = _mapper.Map<AnimalFeatureUserDto>(productFeature);
                    productDto.User =_mapper.Map<AppUserDto>(valuesUserDto);
                    return CreateActionResult(CustomResponseDto<AnimalFeatureUserDto>.Success(200, productDto));
                }

                else if (category.Name =="Special")
                {
                    var productFeature = await _specialProductFeatureService.FindByProductIdAsync(id);
                    if (productFeature == null)
                    {
                        return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Not Find Product"));
                    }
                    var productDto = _mapper.Map<SpecialFeatureUserDto>(productFeature);
                    productDto.User =_mapper.Map<AppUserDto>(valuesUserDto);
                    return CreateActionResult(CustomResponseDto<SpecialFeatureUserDto>.Success(200, productDto));
                }

                else
                {
                    var productFeature = await _belongingProductFeatureService.FindByProductIdAsync(id);
                    if (productFeature==null)
                    {
                        return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Not Find Product"));
                    }
                    var productDto = _mapper.Map<BelongingFeatureUserDto>(productFeature);
                    productDto.User =_mapper.Map<AppUserDto>(valuesUserDto);
                    return CreateActionResult(CustomResponseDto<BelongingFeatureUserDto>.Success(200, productDto));
                }
            }

            if (userproduct==null)
            {
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Kullanıcıya ait ürün bulunamadı"));
            }

            var userproductValDto = _mapper.Map<UserProductDto>(userproduct);



            return CreateActionResult(CustomResponseDto<UserProductDto>.Success(200, userproductValDto));


        }





        //[HttpGet("[action]/{productId}")]
        //public async Task<IActionResult> GetByIdWithAnimal(int productId)
        //{
        //    var product = await _userProductService.geta(productId);
        //    var productDto = _mapper.Map<GetByIdWithAnimalDto>(product);

        //    return CreateActionResult(CustomResponseDto<GetByIdWithAnimalDto>.Success(200, productDto));
        //}

        //[HttpPut("[action]")]
        //public async Task<IActionResult> UpdateAnimalProperty(UpdateAnimalDto dto)
        //{
        //    var product = await _productService.GetByIdAsycn(dto.ProductId);
        //    product.Condition = true;
        //    await _productService.UpdateAsycn(product);

        //    var animal = await _animalService.GetByIdAsycn(Convert.ToInt32(dto.Id));

        //    //animal.Name = dto.Name;
        //    //animal.Age = dto.Age;
        //    //animal.Color = dto.Color;
        //    //animal.UpdatedDate = DateTime.Now;
        //    //animal.Condition = true;
        //    //animal.Birthday = dto.Birthday;
        //    //animal.DiseaseInformation = dto.DiseaseInformation;
        //    //animal.DrugInformation = dto.DrugInformation;
        //    //animal.PassportNumber = dto.PassportNumber;
        //    //animal.Race = dto.Race;
        //    //animal.Type = dto.Type;
        //    //animal.Address1 = dto.Address1;
        //    //animal.Address2 = dto.Address2;
        //    //animal.UpdatedDate = DateTime.Now;
        //    //animal.VaccineInformation = dto.VaccineInformation;
        //    await _animalService.UpdateAsycn(animal);


        //    var userProductValues = new UserProductDto()
        //    {
        //        UserId = dto.UserId,
        //        ProductId = dto.ProductId,
        //    };

        //    //daha önce bir ilişki oluşturlmuşmu
        //    var userproductsin = await _userProductService.GetByIdsAsycn(dto.UserId, dto.ProductId);

        //    if (userproductsin==null)
        //    {
        //        await _userProductService.AddAsycn(_mapper.Map<UserProduct>(userProductValues));
        //    }



        //    return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        //}

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> ViewProduct(int id)
        {
            var product = await _productService.GetByIdAsycn(id);

            var values = await _productService.GetByUserProduct(id);

            var valuesDto = _mapper.Map<GetWithProductDto>(values);

            return CreateActionResult(CustomResponseDto<GetWithProductDto>.Success(200, valuesDto));
        }

        //[HttpGet("[action]")]
        //public async Task<IActionResult> UpdateUserProduct(int productId)
        //{
        //    var values = await _productService.GetByUserProduct(productId);
        //    var valuesDto = _mapper.Map<GetAnimalWithProductId>(values);
        //    return CreateActionResult(CustomResponseDto<GetAnimalWithProductId>.Success(200, valuesDto));
        //}







        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetUserProducts(int userId)
        {
            var products = await _qrCodeService.GetUserProduct(userId);
            var filteredProducts = products.Where(p => p.Product.UserProduct.Any(x => x.UserId == userId));
            if (filteredProducts == null)
            {
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(204, "Ürün bulunamadı"));
            }
            var productDtos = _mapper.Map<List<QRCodeDto>>(filteredProducts);
            return CreateActionResult(CustomResponseDto<List<QRCodeDto>>.Success(200, productDtos));
        }


        //[HttpPost("[action]")]
        //public async Task<IActionResult> ActivePrduct(int productId,int userId)
        //{
        //    var product = await _productService.GetByIdAsycn(productId);
        //    product.Condition = true;
        //    await _productService.UpdateAsycn(product);


        //    var userProductValues = new UserProductDto()
        //    {
        //        UserId = userId,
        //        ProductId = productId,
        //    };

        //    await _userProductService.AddAsycn(_mapper.Map<UserProduct>(userProductValues));
        //    return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        //}


    }
}
