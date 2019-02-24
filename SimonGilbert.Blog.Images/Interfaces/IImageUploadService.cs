using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SimonGilbert.Blog.Images
{
    public interface IImageUploadService
    {
        Task Upload(IFormFileCollection images, string accountId);

        Task<IEnumerable<UploadedImage>> GetAll(string accountId);
    }
}
