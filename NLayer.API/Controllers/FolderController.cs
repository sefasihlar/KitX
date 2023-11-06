using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace NLayer.API.Controllers
{
    [EnableCors("AllowMyOrigin")]
    [Authorize(AuthenticationSchemes = "Roles")]
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {


        private readonly string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "QRCodePng");



        [HttpGet]
        [Route("DownloadFolder")]
        public async Task<IActionResult> DownloadFolder()
        {
            try
            {
                if (Directory.Exists(folderPath))
                {
                    // Klasörün adını al
                    string currentDate = DateTime.Now.ToString("MM-dd-yyyy");
                    string currentTime = DateTime.Now.ToString("HH-mm-ss"); // Saat, dakika ve saniye iki basamaklı olarak gösterilecek

                    // Klasör adını oluştur
                    string folderName = $"QR-{currentDate}-({currentTime})-Backup";

                    // Zip dosyasının yolu
                    string zipFileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "backup", folderName + ".zip");

                    // Klasörü zip dosyasına dönüştür ve kaydet
                    ZipFile.CreateFromDirectory(folderPath, zipFileName);

                    // Zip dosyasını kullanıcıya indir
                    var response = PhysicalFile(zipFileName, "application/zip", folderName + ".zip");

                    // İndirme işleminden sonra dosyayı 2 dakika sonra sil
                    Task.Delay(TimeSpan.FromMinutes(5)).ContinueWith(_ =>
                    {
                        try
                        {
                            if (System.IO.File.Exists(zipFileName))
                            {
                                System.IO.File.Delete(zipFileName);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Hata durumunda uygun bir işlem yapabilirsiniz
                            Console.WriteLine("Dosya silinirken hata oluştu: " + ex.Message);
                        }
                    });

                    return response;
                }
                else
                {
                    return NotFound("Klasör bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("İndirme hatası: " + ex.Message);
            }
        }

    }

    // Klasörü başka bir klasöre kopyalamak için kullanılan fonksiyon
    //private void DirectoryCopy(string sourceDirName, string destDirName)
    //{
    //    // Klasörleri kopyala
    //    DirectoryInfo dir = new DirectoryInfo(sourceDirName);
    //    DirectoryInfo[] dirs = dir.GetDirectories();

    //    if (!Directory.Exists(destDirName))
    //    {
    //        Directory.CreateDirectory(destDirName);
    //    }

    //    FileInfo[] files = dir.GetFiles();
    //    foreach (FileInfo file in files)
    //    {
    //        string temppath = Path.Combine(destDirName, file.Name);
    //        file.CopyTo(temppath, false);
    //    }

    //    foreach (DirectoryInfo subdir in dirs)
    //    {
    //        string temppath = Path.Combine(destDirName, subdir.Name);
    //        DirectoryCopy(subdir.FullName, temppath);
    //    }
    //}


    //[HttpGet]
    //[Route("backup/{folderName}")]
    //public IActionResult ShowBackupFolder(string folderName)
    //{
    //    try
    //    {
    //        // wwwroot içindeki "backup" klasörünün yolu
    //        string backupFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "backup");

    //        // Gösterilecek klasörün tam yolu
    //        string folderPath = Path.Combine(backupFolderPath, folderName);

    //        // Klasör varsa içeriğini göster
    //        if (Directory.Exists(folderPath))
    //        {
    //            // Klasörün içeriğini listele
    //            string[] files = Directory.GetFiles(folderPath);
    //            string[] directories = Directory.GetDirectories(folderPath);

    //            var folderContents = new
    //            {
    //                Files = files,
    //                Directories = directories
    //            };

    //            return new JsonResult(folderContents);
    //        }
    //        else
    //        {
    //            return NotFound("Klasör bulunamadı.");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest("Gösterme hatası: " + ex.Message);
    //    }
    //}




}

