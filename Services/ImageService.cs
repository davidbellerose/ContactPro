using ContactPro.Services.Interfaces;

namespace ContactPro.Services
{
    public class ImageService : IImageService
    {
        private readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
        private readonly string defaultImage = "img/defaultUserImage.png";
        public string ConvertByteArrayToFileAsync(byte[] fileData, string extension)
        {
            if (fileData is null)
            {
                return defaultImage;
            }

            try
            {
                string imageBase64Data = Convert.ToBase64String(fileData);
                return string.Format($"data:img/{extension};base64,{imageBase64Data}");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            try
            {
                using MemoryStream memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                byte[] byteFile = memoryStream.ToArray();
                memoryStream.Close();
                memoryStream.Dispose();

                return byteFile;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
