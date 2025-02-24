using Microsoft.AspNetCore.Http;

namespace AuthDotNetPG.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string RefId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CustomerIP { get; set; }
        public string PONumber { get; set; }
        public string OrderStatus { get; set; } = "Pending";

        // Foreign Keys
        public int? TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public Customer Customer { get; set; }
        public BillingAddress BillTo { get; set; }
        public ShippingAddress ShipTo { get; set; }
        public ICollection<LineItem> LineItems { get; set; }
        public Tax Tax { get; set; }
        public ICollection<UserField>? UserFields { get; set; }

        // Constructor to auto-generate RefId and PONumber
        // Constructor to auto-generate RefId and PONumber
        public Order()
        {
            RefId = GenerateRefId();
            PONumber = GeneratePONumber();
        }

        private static string GenerateRefId()
        {
            return $"REF-{Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper()}";
        }

        private static string GeneratePONumber()
        {
            var random = new Random();
            int randomPart = random.Next(1000, 9999); // Add randomness to reduce collision risk
            return $"PO-{DateTime.UtcNow:yyyyMMddHHmmss}-{randomPart}";
        }
    }
}
