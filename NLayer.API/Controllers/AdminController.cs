using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.AdminDtos.AdminAccountDtos;
using NLayer.Core.DTOs.AnimalDtos;
using NLayer.Core.DTOs.FeatureDtos.AnimalProductFeatureDtos;
using NLayer.Core.DTOs.FeatureWithUserDtos.BelongingProductFeatureDtos;
using NLayer.Core.DTOs.FeatureWithUserDtos.PersonProductFeatureDtos;
using NLayer.Core.DTOs.FeatureWithUserDtos.SpecialProductFeatureDtos;
using NLayer.Core.DTOs.ProductDtos;
using NLayer.Core.Services;
using System.Globalization;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : CustomBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAnimalProductFeatureService _animalProductFeatureService;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ICategoryService _categoryService;
        private readonly IIPAddressService _ipAddressService;
       
        private readonly ILocationservicecs _locationServicecs;
        private readonly IQRCodeService _qrCodeService;
        private readonly IUserProductService _userProductService;
        private readonly IAnimalPhotoService _animalPhotoService;
        private readonly IPersonelProductFeatureService _personelProductFeatureService;
        private readonly IBelongingProductFeatureService _bayingProductFeatureService;
        private readonly ISpecialProductFeatureService _specialProductFeatureService;
        private readonly ILogger<AccountController> _logger;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public AdminController(UserManager<AppUser> userManager, IMapper mapper, IAnimalProductFeatureService animalProductFeatureService, IPersonelProductFeatureService personelProductFeatureService, IBelongingProductFeatureService bayingProductFeatureService, ISpecialProductFeatureService specialProductFeatureService, IProductService productService, RoleManager<AppRole> roleManager, ICategoryService categoryService, IIPAddressService ipAddressService, ILocationservicecs locationServicecs, IQRCodeService qrCodeService, IUserProductService userProductService, IAnimalPhotoService animalPhotoService, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _animalProductFeatureService = animalProductFeatureService;
            _personelProductFeatureService = personelProductFeatureService;
            _bayingProductFeatureService = bayingProductFeatureService;
            _specialProductFeatureService = specialProductFeatureService;
            _productService = productService;
            _roleManager = roleManager;
            _categoryService = categoryService;
            _ipAddressService = ipAddressService;
            _locationServicecs = locationServicecs;
            _qrCodeService = qrCodeService;
            _userProductService = userProductService;
            _animalPhotoService = animalPhotoService;
            _logger = logger;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserList()
        {
            try
            {
                var values = _userManager.Users.ToList();
                var valuesDto = _mapper.Map<List<AdminAccountDto>>(values);
                return CreateActionResult(CustomResponseDto<List<AdminAccountDto>>.Success(200, valuesDto));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "An unexpected error occurred while getting user list");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetWithCategoryListProduct()
        {
            try
            {
                var animal = await _animalProductFeatureService.GetAllAsycn();
                var animalDto = _mapper.Map<List<AnimalProductFeatureDto>>(animal);
                var person = await _personelProductFeatureService.GetAllAsycn();
                var personDto = _mapper.Map<List<PersonProductFeatureDto>>(person);
                var belonging = await _bayingProductFeatureService.GetAllAsycn();
                var belongingDto = _mapper.Map<List<BelongingProductFeatureDto>>(belonging);
                var special = await _specialProductFeatureService.GetAllAsycn();
                var specialDto = _mapper.Map<List<SpecialProductFeatureDto>>(special);

                var values = new GetWithCategoryListProductDto()
                {
                    Animals = animalDto,
                    Persons = personDto,
                    Belonging = belongingDto,
                    Specials = specialDto
                };

                return CreateActionResult(CustomResponseDto<GetWithCategoryListProductDto>.Success(200, values));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "An unexpected error occurred while getting product list with categories");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> UserRegisterDateFilter()
        {
            try
            {
                var users = _userManager.Users.ToList();

                var userRegisterCounts = users
                    .Select(user => new
                    {
                        Date = DateTime.Parse(Convert.ToString(user.CreatedDate)), // CreatedDate'i bir DateTime olarak dönüştür
                        User = user
                    })
                    .Select(data => new
                    {
                        Year = data.Date.Year,
                        Month = data.Date.Month,
                        User = data.User
                    })
                    .GroupBy(user => new { user.Year, user.Month })
                    .Select(group => new UserRegisterByMonthDto
                    {
                        Year = group.Key.Year,
                        Month = group.Key.Month,
                        UserCount = group.Count()
                    })
                    .ToList();

                // Tüm ayları içeren bir liste oluştur
                var allMonths = Enumerable.Range(1, 12);

                var result = allMonths
                    .GroupJoin(userRegisterCounts,
                        month => month,
                        userCount => userCount.Month,
                        (month, counts) => new UserRegisterByMonthDto
                        {
                            Year = DateTime.Now.Year, // Burada yılı istediğiniz şekilde ayarlayabilirsiniz
                            Month = month,
                            UserCount = counts.DefaultIfEmpty(new UserRegisterByMonthDto { UserCount = 0 }).First().UserCount
                        })
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "An unexpected error occurred while filtering user registration dates");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ProductDateTimeFilter()
        {
            try
            {
                var products = await _productService.GetAllAsycn();

                var productCounts = products
                    .Select(product => new
                    {
                        Date = DateTime.Parse(Convert.ToString(product.CreatedDate)),
                        IsActive = product.Condition,
                    })
                    .Select(data => new
                    {
                        Year = data.Date.Year,
                        Month = data.Date.Month,
                        IsActive = data.IsActive
                    })
                    .GroupBy(product => new { product.Year, product.Month, product.IsActive })
                    .Select(group => new
                    {
                        Year = group.Key.Year,
                        Month = group.Key.Month,
                        IsActive = group.Key.IsActive,
                        ProductCount = group.Count()
                    })
                    .ToList();

                // Tüm ayları içeren bir liste oluştur
                var allMonths = Enumerable.Range(1, 12);

                var result = allMonths
                    .Select(month => new ProductByMonthDto
                    {
                        Year = DateTime.Now.Year, // Burada yılı istediğiniz şekilde ayarlayabilirsiniz
                        Month = month,
                        CreatedCount = productCounts
                            .Where(pc => pc.Month == month && pc.IsActive == false)
                            .Select(pc => pc.ProductCount)
                            .FirstOrDefault(),
                        ActivatedCount = productCounts
                            .Where(pc => pc.Month == month && pc.IsActive == true)
                            .Select(pc => pc.ProductCount)
                            .FirstOrDefault()
                    })
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "An unexpected error occurred while filtering product creation dates");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> TotalCountArea()
        {
            try
            {
                var role = _roleManager.Roles.Count();
                var category = await _categoryService.GetAllAsycn();
                var categoryCount = category.Count();
                var ipAddress = await _ipAddressService.GetAllAsycn();
                var ipCount = ipAddress.Count();
                var Location = await _locationServicecs.GetAllAsycn();
                var locationCount = Location.Count();
                var qr = await _qrCodeService.GetAllAsycn();
                var qrCount = qr.Count();
                var userproduct = await _userProductService.GetAllAsycn();
                var photo = await _animalPhotoService.GetAllAsycn();
                var photocount = photo.Count();
                var userproductCount = userproduct.Count();
                var product = await _productService.GetAllAsycn();
                var productCount = product.Count();

                var animal = await _animalProductFeatureService.GetAllAsycn();
                var valuesCount = animal.Count();
                var person = await _personelProductFeatureService.GetAllAsycn();
                var personCount = person.Count();
                var belonging = await _bayingProductFeatureService.GetAllAsycn();
                var belongingCount = belonging.Count();
                var special = await _specialProductFeatureService.GetAllAsycn();
                var specialCount = special.Count();

                var values = new CategoryCount()
                {
                    TotalRole = role,
                    TotalCategory = categoryCount,
                    TotalIPAddress = ipCount,
                    TotalLocations = locationCount,
                    TotalQRCode = qrCount,
                    TotalProduct = productCount,
                    TotalProductPhotos = photocount,
                    TotalUserProduct = userproductCount,
                    TotalPersonCount = personCount,
                    TotalAnimalCount = valuesCount,
                    TotalBelongingCount = belongingCount,
                    TotalSpecialCount = specialCount,
                };

                return Ok(values);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "An unexpected error occurred while calculating total counts");
            }
        }

        private IActionResult HandleError(Exception ex, string message)
        {
            // Genel hata durumu
            _logger.LogError(ex, "{message}. Details: {details}", message, ex.Message);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(500, message));
        }


        public class UserRegisterByMonthDto
        {
            public int Month { get; set; }
            public int Year { get; set; }
            public int CreatedCount { get; set; }
            public int ActivatedCount { get; set; }
            public int UserCount { get; set; }
        }

        public class ProductByMonthDto
        {
            public int Month { get; set; }
            public int Year { get; set; }
            public int CreatedCount { get; set; }
            public int ActivatedCount { get; set; }

        }

        public class CategoryCount
        {
            public int TotalRole { get; set; }
            public int TotalCategory { get; set; }
            public int TotalIPAddress { get; set; }
            public int TotalLocations { get; set; }
            public int TotalProduct { get; set; }
            public int TotalQRCode { get; set; }
            public int TotalUserProduct { get; set; }
            public int TotalProductPhotos { get; set; }
            public int TotalAnimalCount { get; set; }
            public int TotalPersonCount { get; set; }
            public int TotalBelongingCount { get; set; }
            public int TotalSpecialCount { get; set; }
        }


    }
}
