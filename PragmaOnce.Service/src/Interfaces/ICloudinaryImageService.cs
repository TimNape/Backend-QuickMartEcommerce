using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace PragmaOnce.Service.src.Interfaces
{
    public interface ICloudinaryImageService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file);
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}