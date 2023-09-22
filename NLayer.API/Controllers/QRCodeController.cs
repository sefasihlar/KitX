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
    public class QRCodeController : CustomBaseController
    {
        private readonly IProductService _productService;
        private readonly IAnimalService _animalService;

        public QRCodeController(IProductService productService, IAnimalService animalService)
        {
            _productService=productService;
            _animalService=animalService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateQrCode(int length)
        {
            for (int i = 1; i <= length; i++)
            {
                var animal = new Animal();
                var product = new Product();

                await _animalService.AddAsycn(animal);
                product.AnimalId = animal.Id;
                var val = await _productService.AddAsycn(product);

                var data = await _productService.QrCodeToProductAsync(val.Id);

                // Dosya adını ürün ID'sini kullanarak oluşturun.
                var fileName = $"{val.Id}.png";

                // Dosyayı wwwroot/QRCodePng klasörüne kaydedin.
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "QRCodePng", fileName);
                // Dosyayı kaydedin.
                await System.IO.File.WriteAllBytesAsync(filePath, data);

                // Seri numarasını oluşturmak için basit bir seri numara kullanın
                var serialNumber = GenerateSimpleSerialNumber();

                // Ürün bilgilerini güncelleyin
                val.CreatedDate = DateTime.Now;
                val.UpdatedDate = DateTime.Now;
                val.Code = GenerateUniqueCode();
                val.SerialNumber = serialNumber;
                val.Condition = false;
                val.ImageUrl = fileName;

                await _productService.UpdateAsycn(val);
            }

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
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
