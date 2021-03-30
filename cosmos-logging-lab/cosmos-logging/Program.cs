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
    }
}