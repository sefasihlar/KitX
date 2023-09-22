using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.AnimalDtos;
using NLayer.Core.DTOs.AnimalPhotoDtos;
using NLayer.Core.DTOs.IPAddressDtos;
using NLayer.Core.DTOs.ProductDtos;
using NLayer.Core.DTOs.UserDtos;
using NLayer.Core.DTOs.UserProduct;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Service.Services;
using System.Runtime.CompilerServices;

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
        private readonly IMapper _mapper;

     
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIPAddressService _ipAddressService;


        public ProductController(IProductService productService, IMapper mapper, IAnimalService animalService, IAppUserRepository appUserRepository, IUserProductService userProductService, IHttpContextAccessor httpContextAccessor, IIPAddressService ipAddressService, IAnimalPhotoService animalPhotoService)
        {
            _productService=productService;
            _mapper=mapper;
            _animalService=animalService;
            _appUserRepository=appUserRepository;
            _userProductService=userProductService;
            _httpContextAccessor=httpContextAccessor;
            _ipAddressService=ipAddressService;
            _animalPhotoService=animalPhotoService;
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
            var valuesDto =  _mapper.Map<ProductDto>(values);
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

            var ipAdressValues = new IPAddressDto()
            {
                IPAdress = ipAddress,
                ProductId = id
            };

            await _ipAddressService.AddAsycn(_mapper.Map<IPAddress>(ipAdressValues));

            var product = await _productService.GetByUserProduct(id);
            var valuesDto = _mapper.Map<GetWithProductDto>(product);
       
            return CreateActionResult(CustomResponseDto<GetWithProductDto>.Success(200, valuesDto));

        }

        [HttpGet("[action]/{productId}")]
        public async Task<IActionResult> GetByIdWithAnimal(int productId)
        {
            var product = await _productService.GetAnimalWithProductId(productId);
            var productDto = _mapper.Map<GetByIdWithAnimalDto>(product);

            return CreateActionResult(CustomResponseDto<GetByIdWithAnimalDto>.Success(200, productDto));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateAnimalProperty(UpdateAnimalDto dto)
        {
            var product = await _productService.GetByIdAsycn(dto.ProductId);
            product.Condition = true;
            await _productService.UpdateAsycn(product);

            var animal = await _animalService.GetByIdAsycn(Convert.ToInt32(dto.Id));

            animal.Name = dto.Name;
            animal.Age = dto.Age;
            animal.Color = dto.Color;
            animal.UpdatedDate = DateTime.Now;
            animal.Condition = true;
            animal.Birthday = dto.Birthday;
            animal.DiseaseInformation = dto.DiseaseInformation;
            animal.DrugInformation = dto.DrugInformation;
            animal.PassportNumber = dto.PassportNumber;
            animal.Race = dto.Race;
            animal.Type = dto.Type;
            animal.VaccineInformation = dto.VaccineInformation;
            await _animalService.UpdateAsycn(animal);


            var userProductValues = new UserProductDto()
            {
                UserId = dto.UserId,
                ProductId = dto.ProductId,
            };

            //daha önce bir ilişki oluşturlmuşmu
            var userproductsin = await _userProductService.GetByIdsAsycn(dto.UserId, dto.ProductId);

            if (userproductsin==null)
            {
                await _userProductService.AddAsycn(_mapper.Map<UserProduct>(userProductValues));
            }



            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

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
            var products = await _productService.GetProductWithUserId(userId);
            var filteredProducts = products.Where(p => p.UserProducts.Any(up => up.UserId == userId)).ToList();
            var productDtos = _mapper.Map<List<ProductDto>>(filteredProducts);
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productDtos));
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
