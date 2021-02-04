using System;
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

namespace Incident
{
    public static class IncidentsClass
    {
        [FunctionName("GetIncidents")]
        public static async Task<IActionResult> GetIncidents(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "incidents")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            string test = "Nog niet gereed"; // TODO delete becasue of sample


            IncidentContext db = new IncidentContext();

            return new OkObjectResult(JsonConvert.SerializeObject(db.Incidents));
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
    }
}
