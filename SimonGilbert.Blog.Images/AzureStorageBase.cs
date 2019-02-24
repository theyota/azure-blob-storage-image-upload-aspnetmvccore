using Microsoft.WindowsAzure.Storage.Table;

namespace SimonGilbert.Blog.Images
{
    public abstract class AzureStorageBase
    {
        protected string GeneratePartitionKeyFilter(string partitionKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition(
                "PartitionKey", 
                QueryComparisons.Equal, 
                partitionKey);

            return partitionKeyFilter;
        }
    }
}
