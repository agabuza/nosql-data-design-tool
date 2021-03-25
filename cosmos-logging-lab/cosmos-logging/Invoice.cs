
namespace cosmosLogging 
{
    public class Invoice 
    {
        public long InvoiceId { get; set; } 
        public long Id { get; set; }
        public Dictionary<string, string> InvoiceData { get; set; }
    }
}