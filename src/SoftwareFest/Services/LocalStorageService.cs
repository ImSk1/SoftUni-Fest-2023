namespace SoftwareFest.Services
{
    using SoftwareFest.Services.Contracts;

    public class LocalStorageService : IFileService
    {
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
