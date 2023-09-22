using NLayer.Core.Services;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace NLayer.Service.Services
{
    public class QRCodeService : IQRCodeService
    {

        public byte[] GenerateQrCode(string text)
        {
            // QR kod oluşturma
            // QR kod oluşturma
            QRCodeGenerator codeGenerator = new QRCodeGenerator();
            QRCodeData data = codeGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(data);

            // QR kodunun ana rengini siyah yapın
            byte[] blackColor = new byte[] { 0, 0, 0 };

            // Arka plan rengini beyaz yapabilirsiniz
            byte[] whiteColor = new byte[] { 255, 255, 255 };

            // QR kodu alın
            byte[] qrCodeImage = qrCode.GetGraphic(10, blackColor, whiteColor);

            // Kare şeklinde bir resim oluşturun
            int size = qrCodeImage.Length / 4;
            Bitmap squareQrCode = new Bitmap(size, size);

            using (Graphics graphics = Graphics.FromImage(squareQrCode))
            {
                // QR kodun etrafını doldurarak süsleyin
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 0, 0))) // Kırmızı renk
                {
                    graphics.FillRectangle(brush, 0, 0, size, size);
                }

                // QR kodun görüntüsünü çizin
                using (MemoryStream ms = new MemoryStream(qrCodeImage))
                {
                    using (Image qrImage = Image.FromStream(ms))
                    {
                        graphics.DrawImage(qrImage, 0, 0, size, size);
                    }
                }

                // Orta kısmına "X" yazısı ekleyin
                using (Font font = new Font("Arial", 24, FontStyle.Bold))
                using (StringFormat stringFormat = new StringFormat())
                using (SolidBrush textBrush = new SolidBrush(Color.Black))
                {
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    RectangleF textRect = new RectangleF(0, 0, size, size);
                    graphics.DrawString("X", font, textBrush, textRect, stringFormat);
                }
            }

            // Kare şeklindeki QR kodu baytlara dönüştürün
            using (MemoryStream squareQrCodeStream = new MemoryStream())
            {
                squareQrCode.Save(squareQrCodeStream, ImageFormat.Png);
                return squareQrCodeStream.ToArray();
            }
        }

    }


    //public static void Main(string[] args)
    //{
    //    string text = "KitX"; // QR kod olarak oluşturulacak metin
    //    byte[] circularQrCode = GenerateQrCode(text);
    //    File.WriteAllBytes("circular_qr_code.png", circularQrCode);
    //}



}

