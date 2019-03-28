using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SimonGilbert.Blog.Images
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly IImageUploadRepository _repository;
        private readonly CloudBlobContainer _cloudBlobContainer;

        public ImageUploadService(
            IImageUploadRepository repository,
            CloudBlobContainer cloudBlobContainer)
        {
            this._repository = repository;
            this._cloudBlobContainer = cloudBlobContainer;
        }

        public async Task Upload(IFormFileCollection images, string accountId)
        {
            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var fileName = GetFileName(image);

                    var imageName = GenerateImageName(fileName);

                    var imageBytes = ConvertImageToByteArray(image);

                    var imageUrl = await UploadImageByteArray(
                        imageBytes,
                        imageName,
                        image.ContentType);

                    await CreateTableStorageRowForImage(
                        accountId,
                        imageName, 
                        imageUrl,
                        image.ContentType);
                }
            }
        }

        public async Task<IEnumerable<UploadedImage>> GetAll(string accountId)
        {
            var result = await _repository.GetAll(accountId);

            return result;
        }

        private async Task<string> UploadImageByteArray(
            byte[] imageBytes,
            string imageName,
            string contentType)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return null;
            }

            var cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(imageName);

            cloudBlockBlob.Properties.ContentType = contentType;

            const int byteArrayStartIndex = 0;

            await cloudBlockBlob.UploadFromByteArrayAsync(
                imageBytes,
                byteArrayStartIndex,
                imageBytes.Length);

            var imageFullUrlPath = cloudBlockBlob.Uri.ToString();

            return imageFullUrlPath;
        }

        private async Task<TableResult> CreateTableStorageRowForImage(
            string accountId, 
            string imageName, 
            string imageUrl, 
            string contentType)
        {
            var model = new UploadedImage
            {
                PartitionKey = accountId,
                RowKey = Guid.NewGuid().ToString(),
                Name = imageName,
                Url = imageUrl,
                ContentType = contentType,
            };

            var result = await _repository.Create(model);

            return result;
        }

        private byte[] ConvertImageToByteArray(IFormFile image)
        {
            byte[] result = null;

            using (var fileStream = image.OpenReadStream())
            using (var memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                result = memoryStream.ToArray();
            }

            return result;
        }

        private string GetFileName(IFormFile image)
        {
            var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition)
                        .FileName.Trim('"');

            return fileName;
        }

        private string GenerateImageName(string fileName)
        {
            var imageName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(fileName)}";

            return imageName;
        }
    }
}
