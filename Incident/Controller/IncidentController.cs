using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Incident.Data;
using Incident.Models;
using System.Linq;
using System.Collections.Generic;

namespace Incident.App.Controller
{
    public static class IncidentsController
    {
        [FunctionName("GetIncidents")]
        public static IActionResult GetIncidents(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "incidents")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            IncidentContext db = new IncidentContext();
            List<IncidentModel> incidents = db.Incidents.ToList();

            return new OkObjectResult(JsonConvert.SerializeObject(incidents));
        }

        [FunctionName("GetIncident")]
        public static async Task<IActionResult> GetIncident(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "incidents/{id}")] HttpRequest req, int id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            IncidentContext db = new IncidentContext();

            return new OkObjectResult(JsonConvert.SerializeObject(db.Incidents.Where(x => x.Id == id).FirstOrDefault()));
        }

        [FunctionName("PostIncident")]
        public static async Task<IActionResult> PostIncident(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "incidents")] HttpRequest req,
             ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<IncidentModel>(requestBody);

            IncidentContext db = new IncidentContext();

            db.Incidents.Add(data);
            db.SaveChanges();

            return new OkObjectResult(JsonConvert.SerializeObject(db.Incidents));
        }

        [FunctionName("PutIncident")]
        public static async Task<IActionResult> PutIncident(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "incidents")] HttpRequest req,
             ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<IncidentModel>(requestBody);

            IncidentContext db = new IncidentContext();

            db.Incidents.Update(data);
            await db.SaveChangesAsync();

            return new OkObjectResult(JsonConvert.SerializeObject(db.Incidents));
        }

        [FunctionName("DeleteIncident")]
        public static async Task<IActionResult> DeleteIncidents(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "incidents/{id}")] HttpRequest req, int id, 
             ILogger log)
        {
            IncidentContext db = new IncidentContext();
            var delete = await db.Incidents.FindAsync(id);
            if (delete == null)
            {
                log.LogWarning($"Item {id} not found");
                return new BadRequestObjectResult($"Item {id} not found");
            }

            db.Incidents.Remove(delete);
            await db.SaveChangesAsync();

            return new OkResult();
        }
        /*
        [FunctionName("Upload")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Upload")] HttpRequest req, string id,
            ILogger log, ExecutionContext context)
        {
            log.LogInformation($"C# Http trigger function executed at: {DateTime.Now}");
            CreateContainerIfNotExists(log, context);

            CloudStorageAccount storageAccount = GetCloudStorageAccount(context);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("incident-melder-media");

            string idImage = id;
            CloudBlockBlob blob = container.GetBlockBlobReference(idImage);
            //TODO add image function with the name of the id
            var serializeJsonObject = JsonConvert.SerializeObject(new { ID = idImage, Content = $"<html><body><h2> This is a Sample email content! </h2></body></html>" });
            blob.Properties.ContentType = "application/json";

            using (var ms = new MemoryStream())
            {
                LoadStreamWithJson(ms, serializeJsonObject);
                await blob.UploadFromStreamAsync(ms);
            }
            log.LogInformation($"Bolb {idImage} is uploaded to container {container.Name}");
            await blob.SetPropertiesAsync();
            //TODO end
            return new OkObjectResult("Upload function executed successfully!!");
        }

        private static void CreateContainerIfNotExists(ILogger logger, ExecutionContext executionContext)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount(executionContext);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            string[] containers = new string[] { "incident-melder-media" };
            foreach (var item in containers)
            {
                CloudBlobContainer blobContainer = blobClient.GetContainerReference(item);
                blobContainer.CreateIfNotExistsAsync();
            }
        }

        private static CloudStorageAccount GetCloudStorageAccount(ExecutionContext executionContext)
        {
            var config = new ConfigurationBuilder()
                            .SetBasePath(executionContext.FunctionAppDirectory)
                            .AddJsonFile("local.settings.json", true, true)
                            .AddEnvironmentVariables().Build();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config["CloudStorageAccount"]);
            return storageAccount;
        }
        private static void LoadStreamWithJson(Stream ms, object obj)
        {
            StreamWriter writer = new StreamWriter(ms);
            writer.Write(obj);
            writer.Flush();
            ms.Position = 0;
        }
        */
    }
}
