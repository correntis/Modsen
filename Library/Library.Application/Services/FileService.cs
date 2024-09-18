using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Library.Core.Abstractions;

namespace Library.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private string[] allowedExtensions = [".jpg", ".jpeg", ".png"];
        public string DefaultImagePath => "default_image.jpg";

        public FileService(
            IWebHostEnvironment environment
            )
        {
            _environment = environment;
        }

        public async Task<string> SaveAsync(IFormFile imageFile)
        {
            ArgumentNullException.ThrowIfNull(imageFile);

            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, "Uploads");

            var ext = Path.GetExtension(imageFile.FileName);
            if(!allowedExtensions.Contains(ext))
            {
                throw new ArgumentException($"Only {string.Join(",", allowedExtensions)} are allowed");
            }

            var fileName = $"{Guid.NewGuid()}{ext}";
            var fileNameWithPath = Path.Combine(path, fileName);

            using var stream = new FileStream(fileNameWithPath, FileMode.Create);
            await imageFile.CopyToAsync(stream);
            return fileName;
        }

        public void Delete(string fileNameWithExtension)
        {
            if(string.IsNullOrEmpty(fileNameWithExtension))
            {
                throw new ArgumentNullException(nameof(fileNameWithExtension));
            }

            if (fileNameWithExtension == DefaultImagePath)
            {
                return;
            }

            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, $"Uploads", fileNameWithExtension);

            if(!File.Exists(path))
            {
                throw new FileNotFoundException($"Invalid file path");
            }

            File.Delete(path);
        }

    }
}
