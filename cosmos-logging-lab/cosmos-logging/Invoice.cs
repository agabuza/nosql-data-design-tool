
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace cosmosLogging 
{
    public class Invoice 
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        public string InvoiceId { get; set; } 
        public long CustomerId { get; set; }
        public IEnumerable<InvoiceDataPoint> InvoiceData { get; set; }
    }
}