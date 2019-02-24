using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace SimonGilbert.Blog.Images
{
    public class ImageUploadRepository : AzureStorageBase, IImageUploadRepository
    {
        private readonly CloudTable _table;

        public ImageUploadRepository(CloudTable table)
        {
            this._table = table;
        }

        public async Task<TableResult> Create(UploadedImage model)
        {
            var result = await _table.ExecuteAsync(TableOperation.Insert(model));

            return result;
        }

        public async Task<IEnumerable<UploadedImage>> GetAll(string accountId)
        {
            var partitionKeyFilter = GeneratePartitionKeyFilter(accountId);

            var query = new TableQuery<UploadedImage>().Where(partitionKeyFilter);

            var result = await this._table.ExecuteQuerySegmentedAsync(query, null);

            return result;
        }
    }
}
