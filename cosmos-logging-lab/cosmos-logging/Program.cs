using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Azure.Cosmos;

namespace cosmosLogging
{
    class Program
    {
private static string dbName = "";
private static string key = @"";
private static string account = @"";
        static void Main(string[] args)
        {
            
            
            var query = "select count(1) from c where c.CustomerId = 1";

            Queries.QueryTestDataAsync(key, dbName, account, "invoices-by-InvoiceId", "/InvoiceId", query).GetAwaiter().GetResult();
            Queries.QueryTestDataAsync(key, dbName, account, "invoices-by-InvoiceId-withIndexes", "/InvoiceId", query).GetAwaiter().GetResult();
            Queries.QueryTestDataAsync(key, dbName, account, "invoices-by-CustomerId", "/CustomerId", query).GetAwaiter().GetResult();

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