using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Abstractions
{
    public interface IFileService
    {
        void Delete(string fileNameWithExtension);
        Task<string> SaveAsync(IFormFile imageFile);
    }
}
