using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName);
        void DeleteFile(string fileUrl);
    }
}
