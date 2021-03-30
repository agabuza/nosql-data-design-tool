using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Azure.Cosmos;

namespace cosmosLogging
{
    public static class Commands
    {
        public static async void GenerateData(
                string key, 
                string dbName, 
                string endpoint, 
                string containerName, 
                string partitionKey,
                int documentsCount,
                int batchSize)
        {
            var options = new CosmosClientOptions() { AllowBulkExecution = true };

            var cosmosClient = new CosmosClient(endpoint, key, options);
            
            Microsoft.Azure.Cosmos.Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(dbName);
            Container idContainer = await database.CreateContainerIfNotExistsAsync(containerName, partitionKey);

            var invoices = GenerateInvoices();

            List<Task> concurrentTasks = new List<Task>();
            for (int i = 0; i < documentsCount / batchSize; i++)
            {
                List<Task> indexedTasks = new List<Task>();
                foreach(var invoice in invoices.Take(batchSize))
                {
                    indexedTasks.Add(idContainer.CreateItemAsync(invoice));
                }
                
                Console.WriteLine($"Starting insertion of {batchSize} documents into {containerName} in {dbName}");            
                await Task.WhenAll(indexedTasks);
                Console.WriteLine($"Completed insertion of {batchSize} documents into {containerName} in {dbName}");            
            }
        }

        private static IEnumerable<Invoice> GenerateInvoices()
        {
            var invoiceDataFaker = new Faker<InvoiceDataPoint>()
               .StrictMode(true)
               .RuleFor(x => x.Name, f => f.Random.AlphaNumeric(3))
               .RuleFor(x => x.Value, f => f.Random.AlphaNumeric(10));
            var faker = new Faker<Invoice>()
              .StrictMode(true)
              .RuleFor(x => x.CustomerId, f => f.Random.Long(1,5))
              .RuleFor(x => x.id, f => Guid.NewGuid().ToString())
              .RuleFor(x => x.InvoiceId, f => Guid.NewGuid().ToString())
              .RuleFor(x => x.InvoiceData, f => invoiceDataFaker.Generate(f.Random.Int(20,40)));
            
            return faker.GenerateForever();

        }
    }
}