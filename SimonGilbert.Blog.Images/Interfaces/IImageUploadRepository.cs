using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace SimonGilbert.Blog.Images
{
    public interface IImageUploadRepository
    {
        Task<TableResult> Create(UploadedImage model);

        Task<IEnumerable<UploadedImage>> GetAll(string accountId);
    }
}
