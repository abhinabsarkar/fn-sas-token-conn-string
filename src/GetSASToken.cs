using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace ValetKeyPattern
{
    // Function returns SAS token for a file within a given Storage Account
    public static class GetSASToken
    {
        [FunctionName("GetSASToken")]
        // Azure Storage connection string in different format. Here it uses the name of an "AzureWebJobs" prefixed app setting. The connection string is to be added in local.settings.json prefixed with Azure WebJobs in the AppSettings in azure portal
        [StorageAccount("saabhiimages")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{blobname}")] HttpRequest req,            
            [Blob("images/{blobname}", FileAccess.Read)] BlobClient blobClient,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (!await blobClient.ExistsAsync()) 
            {
                return new BadRequestObjectResult($"The blob {blobClient.Name} doesn't exists");
            }

            var blobSasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobClient.BlobContainerName, 
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTime.UtcNow.AddMinutes(-5),
                ExpiresOn= DateTime.UtcNow.AddMinutes(5),       
                Protocol = SasProtocol.Https
            };
            blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasUri = blobClient.GenerateSasUri(blobSasBuilder);
            string sasToken = sasUri.ToString().Split('?')[1];

            return new OkObjectResult(sasToken);
        }
    }
}
