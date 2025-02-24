namespace AuthDotNetPG.Models
{
    public class SubsequentAuthInformation
    {
        public int Id { get; set; }
        public string OriginalNetworkTransId { get; set; }
        public decimal OriginalAuthAmount { get; set; }
        public string Reason { get; set; }

        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
    }
}