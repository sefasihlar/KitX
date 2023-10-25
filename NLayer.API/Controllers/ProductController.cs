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

            var userproduct = product.UserProduct.FirstOrDefault(x => x.ProductId == id);

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
                Latitude =Convert.ToString(ipLocationInfo.latitude),
                Longitude = Convert.ToString(ipLocationInfo.longitude),
                Zip_Code = ipLocationInfo.zip_code,
                Time_Zone = ipLocationInfo.time_zone,
                Asn = ipLocationInfo.asn,
                As = ipLocationInfo.As,
                is_proxy = ipLocationInfo.is_proxy,


            };

            if (valuesUserDto.Email !=null)
            {
                await _emailSenderService.SendEmailAsync(valuesUserDto.Email, "Kitiniz Bulundu", "<h1>Test Email</h1>");
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
