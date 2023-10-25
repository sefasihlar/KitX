using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.QRCodeDtos;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Roles")]
    [EnableCors("AllowMyOrigin")]
    [Route("api/[controller]")]
    [ApiController]

    public class QRCodeController : CustomBaseController
    {
        private readonly IProductService _productService;
        private readonly IAnimalService _animalService;
        private readonly IAnimalProductFeatureService _animalProductFeatureService;
        private readonly IQRCodeService _qrService;
        private readonly IQRGeneratorService _qrGeneratorService;
        private readonly ICategoryService _categoryService;
        private readonly IPersonelProductFeatureService _personelProductFeatureService;
        private readonly ISpecialProductFeatureService _specialProductFeatureService;
        private readonly IBelongingProductFeatureService _belongingProductFeatureService;
        private readonly IMapper _mapper;

        public QRCodeController(IProductService productService, IAnimalService animalService, IAnimalProductFeatureService animalProductFeatureService, IQRCodeService qrService, IQRGeneratorService qrGeneratorService, ICategoryService categoryService, IPersonelProductFeatureService personelProductFeatureService, ISpecialProductFeatureService specialProductFeatureService, IBelongingProductFeatureService belongingProductFeatureService, IMapper mapper)
        {
            _productService = productService;
            _animalService = animalService;
            _animalProductFeatureService = animalProductFeatureService;
            _qrService = qrService;
            _qrGeneratorService = qrGeneratorService;
            _categoryService = categoryService;
            _personelProductFeatureService = personelProductFeatureService;
            _specialProductFeatureService = specialProductFeatureService;
            _belongingProductFeatureService = belongingProductFeatureService;
            _mapper = mapper;
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var values = await _qrService.GetAllAsycn();
            var valuesDto = _mapper.Map<List<QRCodeDto>>(values);
            return CreateActionResult(CustomResponseDto<List<QRCodeDto>>.Success(200, valuesDto));
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetQrCodeWithProductId(int productId)
        {
            var values = await _qrService.GetByQrCodeWithProductId(productId);
            var valuesDto = _mapper.Map<QRCodeDto>(values);
            return CreateActionResult(CustomResponseDto<QRCodeDto>.Success(200, valuesDto));
        }


        [HttpPost]
        public async Task<IActionResult> CreateQrCode(int length, int categoryId)
        {


            for (int i = 1; i <= length; i++)
            {
                var product = new Product();
                var qr = new QrCode();
                var animalFeature = new AnimalProductFeature();
                var personFeature = new PersonProductFeature();
                var SpecialFeature = new SpecialProductFeature();
                var BLFeature = new BelongingProductFeature();

                product.CategoryId = categoryId;
                product.CreatedDate = DateTime.Now;
                product.Condition = false;
                await _productService.AddAsycn(product);

                qr.ProductId = product.Id;
                var val = await _qrService.AddAsycn(qr);


                var category = await _categoryService.GetByIdAsycn(categoryId);

                if (category.Name == "Animal")
                {
                    animalFeature.CreatedDate = DateTime.Now;
                    animalFeature.Condition = false;

                    animalFeature.ProductId = product.Id;
                    await _animalProductFeatureService.AddAsycn(animalFeature);
                }

                else if (category.Name == "Person")
                {
                    var exist = personFeature.ProductId = product.Id;

                    personFeature.CreatedDate = DateTime.Now;
                    personFeature.Condition = false;

                    if (exist != null)
                    {
                        var personVal = await _personelProductFeatureService.AddAsycn(personFeature);
                    }

                }

                else if (category.Name == "Special")
                {
                    var exist = SpecialFeature.ProductId = product.Id;
                    SpecialFeature.CreatedDate = DateTime.Now;
                    SpecialFeature.Condition = false;

                    if (exist != null)
                    {
                        var personVal = await _specialProductFeatureService.AddAsycn(SpecialFeature);
                    }
                }

                else
                {
                    BLFeature.CreatedDate = DateTime.Now;
                    BLFeature.Condition = false;
                    BLFeature.ProductId = product.Id;
                    var blval = await _belongingProductFeatureService.AddAsycn(BLFeature);
                }



                var data = await _productService.QrCodeToProductAsync(val.Id);

                // Dosya adını ürün ID'sini kullanarak oluşturduk.
                var fileName = $"{val.Id}.png";

                // Dosyayı wwwroot/QRCodePng klasörüne kaydedin.
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "QRCodePng", fileName);
                // Dosyayı kaydedin.
                await System.IO.File.WriteAllBytesAsync(filePath, data);

                // Seri numarasını oluşturmak için basit bir seri numara kullandık
                var serialNumber = GenerateSimpleSerialNumber();

                // Ürün bilgilerini güncelleyin
                val.CreatedDate = DateTime.Now;
                val.UpdatedDate = DateTime.Now;
                val.Code = GenerateUniqueCode();
                val.SerialNumber = serialNumber;
                val.Condition = false;
                val.ImageUrl = fileName;
                val.CreatedDate = DateTime.Now;
                val.Condition = false;

                await _qrService.UpdateAsycn(val);
            }

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(201));
        }


        // Basit seri numara üretme işlemi
        private string GenerateSimpleSerialNumber()
        {
            // Yalnızca büyük harf ve rakamları içeren bir seri numarası oluşturun
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var serialNumber = new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return serialNumber;
        }


        // Rasgele benzersiz kod üretme işlemi
        private string GenerateUniqueCode()
        {
            // Rasgele bir benzersiz kod üretme mantığını burada uyarlayın.
            // Örnek olarak 6 karakter uzunluğunda rasgele bir dize dönebilirsiniz.
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var uniqueCode = new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return uniqueCode;
        }







    }
}