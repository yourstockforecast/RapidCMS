using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RapidCMS.Example.WebAssembly.AzureFunctions
{
    public static class Function1
    {
        public static class GetAllFunction
        {
            [FunctionName(nameof(GetAllFunction))]
            public static async Task<IActionResult> RunAsync(
                [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "_rapidcms/{collectionAlias}/all")] HttpRequest req,
                string collectionAlias,
                ILogger log)
            {


                return new OkObjectResult("[]");
            }
        }
    }

    public static class NewFunction
    {
        [FunctionName(nameof(NewFunction))]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "_rapidcms/{collectionAlias}/new")] HttpRequest req,
            string collectionAlias,
            ILogger log)
        {


            return new OkObjectResult("{}");
        }
    }
}
