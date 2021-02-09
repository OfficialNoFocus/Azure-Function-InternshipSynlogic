using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

using System.Reflection;
using Microsoft.Azure.WebJobs.Hosting;
using System.Net.Http;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using AzureFunctions.Extensions.Swashbuckle;
using Microsoft.OpenApi;
using AzureFunctions.Extensions.Swashbuckle.Settings;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Incident.App.Controller.SwaggerController;

[assembly: WebJobsStartup(typeof(SwashBuckleStartup))]
namespace Incident.App.Controller
{
    public class SwaggerController
    {
        internal class SwashBuckleStartup : IWebJobsStartup
        {
            public void Configure(IWebJobsBuilder builder)
            {
                //Register the extension
                builder.AddSwashBuckle(Assembly.GetExecutingAssembly(), opts =>
                {
                    opts.SpecVersion = OpenApiSpecVersion.OpenApi2_0;
                    opts.AddCodeParameter = true;
                    opts.PrependOperationWithRoutePrefix = true;
                    opts.Documents = new[]
                    {
                    new SwaggerDocument
                    {
                        Name = "v1",
                        Title = "Swagger document",
                        Description = "Swagger test document",
                        Version = "v2"
                    }
                };
                    opts.Title = "Swagger Test";
                    //opts.OverridenPathToSwaggerJson = new Uri("http://localhost:7071/api/Swagger/json");
                    opts.ConfigureSwaggerGen = (x =>
                    {
                        x.CustomOperationIds(apiDesc =>
                        {
                            return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)
                                ? methodInfo.Name
                                : new Guid().ToString();
                        });
                    });
                });
            }
        }

        [SwaggerIgnore]
        [FunctionName("Swagger")]
        public static Task<HttpResponseMessage> Run(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Swagger/json")] HttpRequestMessage req,
          [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
        {
            return Task.FromResult(swashBuckleClient.CreateSwaggerDocumentResponse(req));
        }

        [SwaggerIgnore]
        [FunctionName("SwaggerUi")]
        public static Task<HttpResponseMessage> Run2(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Swagger/ui")] HttpRequestMessage req,
            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
        {
            return Task.FromResult(swashBuckleClient.CreateSwaggerUIResponse(req, "swagger/json"));
        }
    }
}
