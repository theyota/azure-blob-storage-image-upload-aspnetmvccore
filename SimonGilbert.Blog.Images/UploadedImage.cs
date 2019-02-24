using Microsoft.WindowsAzure.Storage.Table;

namespace SimonGilbert.Blog.Images
{
    public class UploadedImage : TableEntity
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string ContentType { get; set; }
    }
}
