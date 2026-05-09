using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            if (file == null) return null;

            // 1. تحديد مسار الفولدر اللي هنحفظ فيه (wwwroot/uploads/products)
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", folderName);

            // لو الفولدر مش موجود.. نكريته
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // 2. عمل اسم فريد للملف عشان لو يوزرين رفعوا صورتين بنفس الاسم ميمسحوش بعض
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            // 3. حفظ الملف فعلياً على الهارد ديسك
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // 4. إرجاع المسار اللي هيتخزن في الداتابيز (عشان الـ Front-end يعرضه)
            return $"/uploads/{folderName}/{fileName}";
        }
        public void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            // تحويل المسار النسبي لمسار فيزيائي ومسحه
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}