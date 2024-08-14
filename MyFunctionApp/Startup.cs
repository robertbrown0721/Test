using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using System;

[assembly: FunctionsStartup(typeof(MyFunctionsApp.startup))]

namespace MyFunctionsApp
{
    public class startup : FunctionsStartup
    {
        public override void Configuration(IFunctionHostBuilder builder)
        {
            string endpoint = Environment.GetEnvironmentVariable("CosmosDbEndpoint");
            string key = Environment.GetEnvironmentVariable("CosmosDbKey");
            string databaseId = Environment.GetEnvironmentVariable("CosmosDbDatabaseId");
            string containerId = Enviroment.GetEnvironmentVariable("CosmosDbConatinerId");

            CosmosClientBuilder clientBuilder = new CosmosClientBuilder(CosmosClientBuilder(endpoint, key));
            CosmosClient client = ClientBuilder.WithConnectionModeDirect().Build();
            CosmosDbservice cosmosDbservice = new CosmosDbservice(client, databaseId, containerId);
            builder.Service.AddSingleton<ICosmosDbService>(cosmosDbService);

            client.CreationDataIfnotExistAsync(databaseId).GetAwaiter().GetReult();

            client.GetDatabase(databaseId).CreateContainerIfNotExistAsync(containerId, "/id").GetAwaiter().GetReult();
        }
    }
}