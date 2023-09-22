using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.AnimalPhotoDtos;
using NLayer.Core.DTOs.ProductDtos;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalPhotoController : CustomBaseController
    {
        private readonly IAnimalPhotoService _animalPhotoService;
        private readonly IAnimalService _animalService;
        private readonly IMapper _mapper;

        public AnimalPhotoController(IAnimalPhotoService animalPhotoService, IAnimalService animalService, IMapper mapper)
        {
            _animalPhotoService=animalPhotoService;
            _animalService=animalService;
            _mapper=mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var values = await _animalPhotoService.GetAllAsycn();
            var valuesDto = _mapper.Map<List<AnimalPhotoDto>>(values);
            return CreateActionResult(CustomResponseDto<List<AnimalPhotoDto>>.Success(200, valuesDto));
        }



        [HttpGet("[action]")]
        public async Task<IActionResult> GetByAnimalId(int AnimalId)
        {
            var values = await _animalPhotoService.GetAllAsycn();
            var valuesFilter = values.Where(x=>x.AnimalId == AnimalId);
            var valuesDto = _mapper.Map<List<AnimalPhotoDto>>(valuesFilter);
            return CreateActionResult(CustomResponseDto<List<AnimalPhotoDto>>.Success(200, valuesDto));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Delete(int id)
        {
           var values = await _animalPhotoService.GetByIdAsycn(id);
            if (values != null)
            {
                await _animalPhotoService.RemoveAsycn(values);
                return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
            }
              

            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(204,"Silme işlemi başarısız bilgileri gözden geçiriniz"));
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateAnimalPhoto(int animalId, IFormFile newFile)
        {
            try
            {
                var values = await _animalPhotoService.GetAllAsycn();
                var animalphoto = values.FirstOrDefault(x => x.AnimalId == animalId);

                if (animalphoto == null)
                {
                    return NotFound("Belirtilen hayvan bulunamadı.");
                }

                // Mevcut resmin adı ve yolu
                var currentFileName = animalphoto.ImageUrl;
                var currentFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Animal-Photo", currentFileName);

                // Mevcut resmi siliyoruz
                if (System.IO.File.Exists(currentFilePath))
                {
                    System.IO.File.Delete(currentFilePath);
                }

                // Yeni dosyayı kaydetmek için benzersiz bir isim oluşturuyoruz
                var newFileName = Guid.NewGuid().ToString() + "_" + newFile.FileName;
                var newFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Animal-Photo", newFileName);

                // Yeni dosyayı kaydediyoruz
                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await newFile.CopyToAsync(stream);
                }

                // Yeni dosyanın adını ve yolu güncelliyoruz
                animalphoto.ImageUrl = newFileName;
                await _animalPhotoService.UpdateAsycn(animalphoto);

                return Ok("Hayvanın fotoğrafı başarıyla güncellendi.");
            }
            catch (Exception ex)
            {
                // Yakalanan hatayı loglayabilir veya hata mesajını yanıta dahil edebilirsiniz.
                // Örneğin:
                // _logger.LogError(ex, "Hayvanın fotoğrafı güncellenirken bir hata oluştu.");
                return StatusCode(500, "İç sunucu hatası: " + ex.Message);
            }
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> SaveFiles(int animalId, List<IFormFile> files)
        {
            try
            {
                var animal = await _animalService.GetByIdAsycn(animalId);

                if (files == null || files.Count == 0)
                {
                    return BadRequest("Dosya seçilmedi veya dosya boş.");
                }

                // Dosyanın kaydedileceği yolun belirlenmesi
                var webUIPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Animal-Photo");

                // Klasör yoksa oluşturulması
                if (!Directory.Exists(webUIPath))
                {
                    Directory.CreateDirectory(webUIPath);
                }

                foreach (var file in files)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                    var filePath = Path.Combine(webUIPath, uniqueFileName);

                    // Dosyanın kaydedilmesi
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var animalPhotoDto = new AnimalPhotoDto()
                    {
                        AnimalId = animalId,
                        ImageUrl = uniqueFileName,
                        Condition = true

                    };

                    var animalPhotoValues = _mapper.Map<AnimalPhoto>(animalPhotoDto);

                    await _animalPhotoService.AddAsycn(animalPhotoValues);

                    // Her dosya için ilgili işlemleri yapabilirsiniz, örneğin veritabanına kaydedebilirsiniz.
                }

                return Ok("Dosyalar başarıyla kaydedildi.");
            }
            catch (Exception ex)
            {
                // Yakalanan hatayı loglayabilir veya hata mesajını yanıta dahil edebilirsiniz.
                // Örneğin:
                // _logger.LogError(ex, "Dosyalar kaydedilirken bir hata oluştu.");
                return StatusCode(500, "İç sunucu hatası: " + ex.Message);
            }
        }
    }
}
