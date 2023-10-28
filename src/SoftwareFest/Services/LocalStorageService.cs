using Microsoft.AspNetCore.Mvc;
using SoftwareFest.Services.Contracts;

namespace SoftwareFest.Services
{
    public class LocalStorageService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public LocalStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GetFilePath(string fileName)
        {
            string filePath = Path.Combine(Path.Combine(_env.WebRootPath, "localstorage"), fileName);

            return filePath;
        }

        public async Task<string> SaveFile(IFormFile fileData)
        {
            string path = string.Empty;

            if (fileData != null)
            {
                var uploadsFolder = Path.Combine("wwwroot", "localstorage");
                var uniqueFileName = Guid.NewGuid() + "_" + fileData.FileName;
                path = Path.Combine(uploadsFolder, uniqueFileName);

                using var fileStream = new FileStream(path, FileMode.Create);
                await fileData.CopyToAsync(fileStream);
            }

            return path.Remove(0, 7);
        }

        public bool DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);

                return true;
            }

            return false;
        }
    }
}
