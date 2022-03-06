using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace movieAPI.Helpers
{
    public class AzureStorageService : IFileStorageService

    {
        private string connectionString;
        public AzureStorageService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorageConnection");

        }
        public async Task DeleteFile(string fileRoute, string containerName)
        {   
            // if the actor dosnt have a picture we just continue immediately 
            if (string.IsNullOrEmpty(fileRoute))
            {
                return;
            }
            var client = new BlobContainerClient(connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            var fileName = Path.GetFileName(fileRoute);
            var blob = client.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();
        }
            
        // delete a file a recreate a new one 
        public async Task<string> EditFile(string containerName, IFormFile file, string fileRoute)
        {
            await DeleteFile(fileRoute, containerName);
            return await SaveFile(containerName, file);

        }
 // folder that will put all our files together (just like any folder in our machine)
        public async Task<string> SaveFile(string containerName, IFormFile file)
        {

            var client = new BlobContainerClient(connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            // two container one for the picture of the actors, second for the movies
            client.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{ Guid.NewGuid()}{ extension}";
            var blob = client.GetBlobClient(fileName);
            await blob.UploadAsync(file.OpenReadStream());
            return blob.Uri.ToString();
        }
    }
}
