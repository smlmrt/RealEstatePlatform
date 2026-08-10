using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;

namespace RealEstatePlatform.Web.Helpers
{
    public static class ImageHelper
    {
        public static async Task<string> CompressAndAddWatermarkAsync(IFormFile file, string webRootPath)
        {
            // Yüklenecek klasör yolunu belirle: wwwroot/uploads
            var uploadsFolder = Path.Combine(webRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Benzersiz bir dosya adı oluştur (örn: 3a2f81..._ev.jpg)
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = file.OpenReadStream())
            using (var image = await Image.LoadAsync(stream))
            {
                // 1. RESİM BOYUTLANDIRMA & SIKIŞTIRMA
                // Genişliği maksimum 1200px yap, yüksekliği orantılı ayarla
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(1200, 800)
                }));

                // 2. WATERMARK (FILIGRAN / LOGO) EKLEME
                // Sistemdeki varsayılan bir fontu kullan
                var font = SystemFonts.CreateFont("Arial", 36, FontStyle.Bold);
                
                var watermarkText = "© RealEstate Platform";
                
                // Sağ alt köşeye yarı saydam beyaz renkte yazı bas
                image.Mutate(x => x.DrawText(
                    watermarkText,
                    font,
                    Color.FromRgba(255, 255, 255, 128), // 128 = %50 Saydamlık
                    new PointF(image.Width - 400, image.Height - 60)
                ));

                // 3. SIKIŞTIRILMIŞ RESMİ KAYDET (%75 Kalite ile Sıkıştırma)
                await image.SaveAsJpegAsync(filePath, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder
                {
                    Quality = 75
                });
            }

            // Veritabanına kaydedilecek bağıl yol
            return "/uploads/" + uniqueFileName;
        }
    }
}