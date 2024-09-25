using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Library.API.Extensions
{
    public static class FilesExtensions
    {
        public static void AddStaticFiles(this WebApplication app)
        {
            var contentPath = app.Environment.ContentRootPath;
            var path = Path.Combine(contentPath, "Uploads");

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = "/Resources"
            });

        }
    }
}
