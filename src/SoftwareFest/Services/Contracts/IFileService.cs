using Microsoft.AspNetCore.Mvc;

namespace SoftwareFest.Services.Contracts
{
    public interface IFileService
    {
        string GetFilePath(string fileName);
        Task<string> SaveFile(IFormFile fileData);
        bool DeleteFile(string path);
    }
}
