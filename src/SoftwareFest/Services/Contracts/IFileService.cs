using Microsoft.AspNetCore.Mvc;

namespace SoftwareFest.Services.Contracts
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile fileData);
        bool DeleteFile(string path);
    }
}
