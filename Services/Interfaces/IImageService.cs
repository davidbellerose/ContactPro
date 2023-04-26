﻿namespace ContactPro.Services.Interfaces
{
    public interface IImageService
    {
        public Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);

        public string ConvertByteArrayToFileAsync(byte[] fileData, string extension);
    }
}
