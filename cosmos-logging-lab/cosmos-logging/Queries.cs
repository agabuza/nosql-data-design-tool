using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace cosmosLogging 
{
    public static class Queries
    {
        public static async Task QueryTestDataAsync(
                string key, 
                string dbName, 
                string endpoint, 
                string containerName, 
                string partitionKey,
                string query)
        {
            CosmosClient client = new CosmosClient(endpoint, key);
            Database database = await client.CreateDatabaseIfNotExistsAsync(dbName);
            Container container = await database.CreateContainerIfNotExistsAsync(containerName, partitionKey);
            
            using (FeedIterator<Invoice> feedIterator = container.GetItemQueryIterator<Invoice>(query))
            {
                Console.WriteLine($"query: {query} in container {containerName} charge start");
                while (feedIterator.HasMoreResults)
                {
                    var response = await feedIterator.ReadNextAsync();                    
                    Console.WriteLine($"Request charge: {response.RequestCharge}");
                }
                
                Console.WriteLine($"query: {query} in container {containerName} charge finished");
            }
        }
    }