using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MyFunctionApp
{
    public class CosmosDbFunction
    {
        private readonly ICosmosDbService _cosmosDbService;

        public CosmosDbFunction(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [FunctionName("GetItems")]
        public async Task<IActionResult> GetItems(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "items")] HttpRequest req,
            ILogger log)
        {
            var items = await _cosmosDbService.GetItemsAsync("SELECT * FROM c");
            return new OkObjectResult(items);
        }

        [FunctionName("GetItem")]
        public async Task<IActionResult> GetItem(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "items/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            var item = await _cosmosDbService.GetItemAsync(id);
            if (item == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(item);
        }

        [FunctionName("CreateItem")]
        public async Task<IActionResult> CreateItem(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "items")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var item = JsonConvert.DeserializeObject<Item>(requestBody);
            await _cosmosDbService.AddItemAsync(item);
            return new OkObjectResult(item);
        }

        [FunctionName("UpdateItem")]
        public async Task<IActionResult> UpdateItem(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "items/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var item = JsonConvert.DeserializeObject<Item>(requestBody);
            if (id != item.Id)
            {
                return new BadRequestResult();
            }

            await _cosmosDbService.UpdateItemAsync(id, item);
            return new OkObjectResult(item);
        }

        [FunctionName("DeleteItem")]
        public async Task<IActionResult> DeleteItem(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "items/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            await _cosmosDbService.DeleteItemAsync(id);
            return new OkResult();
        }
    }
}